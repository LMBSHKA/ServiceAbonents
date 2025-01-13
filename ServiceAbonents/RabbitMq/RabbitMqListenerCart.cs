
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceAbonents.Data;
using ServiceAbonents.Debiting;
using ServiceAbonents.Dtos;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ServiceAbonents.RabbitMq
{
    public class RabbitMqListenerCart : BackgroundService
    {
        private static readonly Uri _uri = new Uri("amqps://akmeanzg:TMOCQxQAEWZjfE0Y7wH5v0TN_XTQ9Xfv@mouse.rmq5.cloudamqp.com/akmeanzg");
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMqListenerCart(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var factory = new ConnectionFactory { Uri = _uri };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(exchange: "TransferCart", type: ExchangeType.Topic);
            var queueDeclareResult = await _channel.QueueDeclareAsync(durable: true, exclusive: false,
    autoDelete: false, arguments: null);
            await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
            var queueName = queueDeclareResult.QueueName;
            await _channel.QueueBindAsync(queue: queueName, exchange: "TransferCart", routingKey: "secretKeyCart");

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var abonentData = JsonSerializer.Deserialize<List<TransferDataAbonentDto>>(Encoding.UTF8.GetString(body));

                Console.WriteLine($"[X] Recieved {abonentData}");
                Debug.WriteLine($"Recieved {abonentData}");
                UpdateAbonent(abonentData);

                return Task.CompletedTask;

            };
            await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer);

            Console.ReadLine();
        }

        public void UpdateAbonent(List<TransferDataAbonentDto> dataAbonents)
        {
            if (dataAbonents == null)
                throw new ArgumentNullException(nameof(dataAbonents));

            using (var scope = _scopeFactory.CreateAsyncScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IAbonentRepo>();
                var debiting = scope.ServiceProvider.GetRequiredService<IDebiting>();

                for (int i = 0; i < dataAbonents.Count; i++)
                {
                    if (i == 0)
                        repo.UpdateNewAbonent(dataAbonents[0].AbonentId, SetParametrs(dataAbonents[i]));

                    else
                        repo.CreateSimilarAbonent(dataAbonents[i]);

                    debiting.AddNewAbonent(new DebitingAbonentDto
                    {
                        TariffId = dataAbonents[i].TariffId,
                        Id = dataAbonents[i].AbonentId,
                        Balance = 0,
                        TariffCost = dataAbonents[i].TariffCost
                    });
                }
            }
        }

        public UpdateNewAbonentDto SetParametrs(TransferDataAbonentDto data)
        {
            return new UpdateNewAbonentDto
            {
                TariffId = data.TariffId,
                PhoneNumber = data.PhoneNumber
            };
        }
    }
}

using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Diagnostics;
using ServiceAbonents.Dtos;
using System.Text;
using ServiceAbonents.Data;

namespace ServiceAbonents.RabbitMq
{
    public class RabbitMqListenerTarif : BackgroundService
    {
        private static readonly Uri _uri = new Uri("amqps://akmeanzg:TMOCQxQAEWZjfE0Y7wH5v0TN_XTQ9Xfv@mouse.rmq5.cloudamqp.com/akmeanzg");
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMqListenerTarif(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var factory = new ConnectionFactory { Uri = _uri };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "GetTarif", type: ExchangeType.Topic);
            var queueDeclareResult = await channel.QueueDeclareAsync(durable: true, exclusive: false,
    autoDelete: false, arguments: null);
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
            var queueName = queueDeclareResult.QueueName;
            await channel.QueueBindAsync(queue: queueName, exchange: "GetTarif", routingKey: "secretKeyTarif");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var abonentData = JsonSerializer.Deserialize<GetTarifDto>(Encoding.UTF8.GetString(body));

                Console.WriteLine($"[X] Recieved {abonentData}");
                Debug.WriteLine($"Recieved {abonentData}");
                using (var scope = _scopeFactory.CreateAsyncScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IRemainRepo>();
                    repo.Update(abonentData.AbonentId, new RemainUpdateDto
                    {
                        RemainGb = abonentData.RemainGb,
                        RemainMin = abonentData.RemainMin,
                        RemainSMS = abonentData.RemainSMS,
                        UnlimVideo = abonentData.UnlimVideo,
                        UnlimMusic = abonentData.UnlimMusic,
                        UnlimSocials = abonentData.UnlimSocials,
                        LongDistanceCall = abonentData.LongDistanceCall
                    });

                    return Task.CompletedTask;
                }

            };
            await channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer);

            Console.ReadLine();
        }
    }
}

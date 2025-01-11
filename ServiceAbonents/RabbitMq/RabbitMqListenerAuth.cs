using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Diagnostics;
using System.Text;
using ServiceAbonents.Data;

namespace ServiceAbonents.RabbitMq
{
    public class RabbitMqListenerAuth : BackgroundService
    {
        private static readonly Uri _uri = new Uri("amqps://akmeanzg:TMOCQxQAEWZjfE0Y7wH5v0TN_XTQ9Xfv@mouse.rmq5.cloudamqp.com/akmeanzg");
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMqListenerAuth(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var factory = new ConnectionFactory { Uri = _uri };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "TransferAuth", type: ExchangeType.Topic);
            var queueDeclareResult = await channel.QueueDeclareAsync(durable: true, exclusive: false,
    autoDelete: false, arguments: null);
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
            var queueName = queueDeclareResult.QueueName;
            await channel.QueueBindAsync(queue: queueName, exchange: "TransferAuth", routingKey: "secretKeyAuth");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var phoneNumber = JsonSerializer.Deserialize<string>(Encoding.UTF8.GetString(body));

                Console.WriteLine($"[X] Recieved {phoneNumber}");
                Debug.WriteLine($"Recieved {phoneNumber}");
                using (var scope = _scopeFactory.CreateAsyncScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IAbonentRepo>();
                    repo.GetAbonentByPhoneNumber(phoneNumber);
                }
                    return Task.CompletedTask;

            };
            await channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer);

            Console.ReadLine();
        }
    }
}

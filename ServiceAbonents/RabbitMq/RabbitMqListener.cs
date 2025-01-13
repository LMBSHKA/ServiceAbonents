using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceAbonents.Debiting;
using ServiceAbonents.Dtos;
using System.Diagnostics;
using System.Text;
using System.Text.Json;


namespace ServiceAbonents.RabbitMq
{
    public class RabbitMqListener : BackgroundService
    {
        private static readonly Uri _uri = new Uri("amqps://akmeanzg:TMOCQxQAEWZjfE0Y7wH5v0TN_XTQ9Xfv@mouse.rmq5.cloudamqp.com/akmeanzg");
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMqListener(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var factory = new ConnectionFactory { Uri = _uri };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(exchange: "OperationWithBalance", type: ExchangeType.Topic);
            var queueDeclareResult = await _channel.QueueDeclareAsync(durable: true, exclusive: false,
    autoDelete: false, arguments: null);
            await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
            var queueName = queueDeclareResult.QueueName;
            await _channel.QueueBindAsync(queue: queueName, exchange: "OperationWithBalance", routingKey: "secretKey");

            var res = false;
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync +=  (model, ea) =>
            {
                using (var scope = _scopeFactory.CreateAsyncScope())
                {
                    var debiting = scope.ServiceProvider.GetRequiredService<IDebiting>();
                    var body = ea.Body.ToArray();
                    var amount = JsonSerializer.Deserialize<TopUpDto>(Encoding.UTF8.GetString(body));

                    Console.WriteLine($"[X] Recieved {amount}");
                    Debug.WriteLine($"Recieved {amount}");
                    res = debiting.ExamTransaction(amount);

                    if (res == false || amount == null)
                    {
                        Console.WriteLine("Error in processing");
                        _channel.BasicNackAsync(ea.DeliveryTag, false, false);
                    }
                    else
                        _channel.BasicAckAsync(ea.DeliveryTag, false);

                    return Task.CompletedTask;
                }
            };
            await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer);

            Console.ReadLine();
        }
    }
}
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceAbonents.Data;
using ServiceAbonents.Dtos;
using System.Diagnostics;
using System.Text;
using System.Text.Json;


namespace ServiceAbonents.RabbitMq
{
    public class RabbitMqListener : BackgroundService
    {
        private static readonly Uri _uri = new Uri("amqps://akmeanzg:TMOCQxQAEWZjfE0Y7wH5v0TN_XTQ9Xfv@mouse.rmq5.cloudamqp.com/akmeanzg");

        
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var factory = new ConnectionFactory { Uri = _uri };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "OperationWithBalance", type: ExchangeType.Topic);
            var queueDeclareResult = await channel.QueueDeclareAsync(durable: true, exclusive: false,
    autoDelete: false, arguments: null);
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
            var queueName = queueDeclareResult.QueueName;
            await channel.QueueBindAsync(queue: queueName, exchange: "OperationWithBalance", routingKey: "secretKey");

            var res = false;
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync +=  (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var type = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["type"]);
                if (type == "Oper")
                    Console.WriteLine("Oper------------------------------------------");
                var amount = JsonSerializer.Deserialize<TopUpDto>(Encoding.UTF8.GetString(body));
                
                Console.WriteLine($"[X] Recieved {amount}");
                Debug.WriteLine($"Recieved {amount}");
                res = Debiting.Debiting.ExamTransaction(amount);

                if (res == false || amount == null)
                {
                    Console.WriteLine("Error in processing");
                    channel.BasicNackAsync(ea.DeliveryTag, false, false);
                    
                }
                else
                    channel.BasicAckAsync(ea.DeliveryTag, false);
                
                return Task.CompletedTask;
            };
            await channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer);

            Console.ReadLine();
        }
    }
}
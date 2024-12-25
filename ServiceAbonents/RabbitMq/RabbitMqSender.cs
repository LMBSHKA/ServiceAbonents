using RabbitMQ.Client;
using ServiceAbonents.Dtos;
using System.Text;
using System.Text.Json;

namespace ServiceAbonents.RabbitMq
{
    public class RabbitMqSender : ISender
    {
        private readonly Uri _uri = new Uri("amqps://akmeanzg:TMOCQxQAEWZjfE0Y7wH5v0TN_XTQ9Xfv@mouse.rmq5.cloudamqp.com/akmeanzg");

        public void SendMessage(object obj)
        {
            var message = JsonSerializer.Serialize(obj);
            SendMessage(message);
        }

        public async Task<bool> SendMessage(TransactionDto transaction)
        {
            var factory = new ConnectionFactory() { Uri = _uri };
            using var connection = await factory.CreateConnectionAsync();
            var channelOpts = new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true);
            using var channel = await connection.CreateChannelAsync(channelOpts);

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await channel.ExchangeDeclareAsync(exchange: "TransferAbonents", type: ExchangeType.Topic);
            var routingKey = "secretKeyTransfer";

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(transaction));

            try
            {
                await channel.BasicPublishAsync(exchange: "TransferAbonents", routingKey: routingKey, body: body);
                Console.WriteLine($"[x] sent {transaction}");
                return true;
            }

            catch
            {
                Console.WriteLine("Message was not sent");
                return false;
            }
        }
    }
}
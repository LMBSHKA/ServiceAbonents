using RabbitMQ.Client;
using ServiceAbonents.Dtos;
using System.Text;
using System.Text.Json;

namespace ServiceAbonents.RabbitMq
{
    public class RabbitMqSender : ISender
    {
        private readonly Uri _uri = new Uri("amqps://akmeanzg:TMOCQxQAEWZjfE0Y7wH5v0TN_XTQ9Xfv@mouse.rmq5.cloudamqp.com/akmeanzg");

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

        public async Task<bool> SendMessage(IdForCartDto idForCart)
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

            await channel.ExchangeDeclareAsync(exchange: "GetDataAbonent", type: ExchangeType.Topic);
            var routingKey = "secretKeyData";

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(idForCart));

            try
            {
                await channel.BasicPublishAsync(exchange: "GetDataAbonent", routingKey: routingKey, body: body);
                Console.WriteLine($"[x] sent {idForCart}");
                return true;
            }

            catch
            {
                Console.WriteLine("Message was not sent");
                return false;
            }
        }

        public async Task<bool> SendMessage(IdForTarifDto idForTarif)
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

            await channel.ExchangeDeclareAsync(exchange: "GetTarif", type: ExchangeType.Topic);
            var routingKey = "secretKeyTarif";

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(idForTarif));

            try
            {
                await channel.BasicPublishAsync(exchange: "GetTarif", routingKey: routingKey, body: body);
                Console.WriteLine($"[x] sent {idForTarif}");
                return true;
            }

            catch
            {
                Console.WriteLine("Message was not sent");
                return false;
            }
        }

        public async Task<bool> SendMessage(TransferForAuthDto data)
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

            await channel.ExchangeDeclareAsync(exchange: "sendAuth", type: ExchangeType.Topic);
            var routingKey = "secretKeySendAuth";

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));

            try
            {
                await channel.BasicPublishAsync(exchange: "sendAuth", routingKey: routingKey, body: body);
                Console.WriteLine($"[x] sent {data}");
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
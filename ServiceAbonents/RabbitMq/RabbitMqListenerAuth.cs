using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Diagnostics;
using System.Text;
using ServiceAbonents.Data;
using MassTransit;
using ServiceAbonents.Dtos;

namespace ServiceAbonents.RabbitMq
{
    public class RabbitMqListenerAuth : IConsumer<TransferForAuthRequestDTO>
    {
        private readonly IAbonentRepo _abonentRepo;

        public RabbitMqListenerAuth(IAbonentRepo abonentRepo)
        {
            _abonentRepo = abonentRepo;
        }

        public async Task Consume(ConsumeContext<TransferForAuthRequestDTO> context)
        {
            var phoneNumber = context.Message.PhoneNumber;

            // Ищем пользователя по номеру телефона
            var user = _abonentRepo.GetAbonentByPhoneNumber(phoneNumber);

            if (user != null)
            {
                // Если нашли, отправляем UserResponseDTO обратно
                var userResponse = new TransferForAuthDto
                {
                    AbonentId = user.AbonentId,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role
                };

                // Отправляем ответ обратно
                await context.RespondAsync(userResponse);
            }
            else
            {
                // Если не нашли, отправляем null или ошибку
                await context.RespondAsync<TransferForAuthDto>(null);
            }
        }
    }
}

using JetBrains.Annotations;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.RabbitMq
{
    public interface ISender
    {
        Task<bool> SendMessage(TransactionDto transaction);
    }
}

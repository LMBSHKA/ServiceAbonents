using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.RabbitMq
{
    public interface ISender
    {
        void SendMessage(object obj);
        Task<bool> SendMessage(TransactionDto transaction);
    }
}

using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.RabbitMq
{
    public interface ISender
    {
        void SendMessage(object obj);
        abstract static Task<bool> SendMessage(TransactionDto transaction);
    }
}

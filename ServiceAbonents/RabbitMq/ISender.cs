using JetBrains.Annotations;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.RabbitMq
{
    public interface ISender
    {
        Task<bool> SendMessage(TransactionDto transaction);
        Task<bool> SendMessage(IdForCartDto idForCart);
        Task<bool> SendMessage(IdForTarifDto idForTarif);
        Task<bool> SendMessage(TransferForAuthDto data);
    }
}

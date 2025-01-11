using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Debiting
{
    public interface IDebiting
    {
        DebitingAbonentDto FindNewAbonent(Guid id);
        DebitingAbonentDto FindOldAbonent(Guid id);
        void AddNewAbonent(DebitingAbonentDto abonent);
        void AddOldAbonent(DebitingAbonentDto abonent);
        void UpdateNewAbonent(DebitingAbonentDto abonent);
        void UpdateOldAbonent(DebitingAbonentDto abonent);
        bool ExamTransaction(TopUpDto newBalance);
        TransactionDto SetTransaction(Guid id, decimal amount);
        void UpdateDate(Guid id);

    }
}

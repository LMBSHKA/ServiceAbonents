using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Debiting
{
    public interface IDebiting
    {
        DebitingAbonentDto FindNewAbonent(int id);
        DebitingAbonentDto FindOldAbonent(int id);
        void AddNewAbonent(DebitingAbonentDto abonent);
        void AddOldAbonent(DebitingAbonentDto abonent);
        void UpdateNewAbonent(DebitingAbonentDto abonent);
        void UpdateOldAbonent(DebitingAbonentDto abonent);
        bool ExamTransaction(TopUpDto newBalance);
        TransactionDto SetTransaction(int id, decimal amount);
        void UpdateDate(int id);

    }
}

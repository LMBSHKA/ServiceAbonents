using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Debiting
{
    public interface IDebiting
    {
        Abonent FindAbonents(int id);
        void AddNewAbonent(Abonent abonent);
        void RemoveAbonent(Abonent abonent);
        void Update(Abonent abonent);
        bool ExamTransaction(TopUpDto newBalance);

    }
}

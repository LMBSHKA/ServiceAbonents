using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Data
{
    public interface IRemainRepo
    {
        bool SaveChanges();
        IEnumerable<Remain> GetAllRemains();
        Remain GetRemainByAbonentId(int id);
        void CreateRemain(Remain newRemain);
        void Update(Remain remain);
    }
}

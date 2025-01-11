using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Data
{
    public interface IRemainRepo
    {
        bool SaveChanges();
        IEnumerable<Remain> GetAllRemains();
        Remain GetRemainByAbonentId(Guid id);
        void CreateRemain(Remain newRemain);
        void Update(Guid clientId, RemainUpdateDto updateRemain);
    }
}

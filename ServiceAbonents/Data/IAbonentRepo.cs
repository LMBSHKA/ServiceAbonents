using Azure;
using Microsoft.AspNetCore.JsonPatch;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Data
{
    public interface IAbonentRepo
    {
        bool SaveChange();
        IEnumerable<User> GetAllAbonents();
        User GetAbonentById(int id);
        void CreateAbonent(User abonent);
        int GetTariffIdByAbonentId(int id);
        void Update(int id, AbonentsUpdateDto updateAbonent);
    }
}

using Azure;
using Microsoft.AspNetCore.JsonPatch;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Data
{
    public interface IAbonentRepo
    {
        bool SaveChange();
        IEnumerable<Abonent> GetAllAbonents();
        Abonent GetAbonentById(int id);
        void CreateAbonent(Abonent abonent);
        int GetTariffIdByAbonentId(int id);
        void Update(Abonent updateAbonent);
    }
}

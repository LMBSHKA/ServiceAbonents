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
        Abonent CreateAbonent(AbonentCreateDto abonent);
        int GetTariffIdByAbonentId(int id);
        void Update(int id, AbonentsUpdateDto updateAbonent);
        void UpdateNewAbonent(int id, UpdateNewAbonentDto newAbonent);
        void CreateSimilarAbonent(TransferDataAbonentDto newAbonent);
    }
}

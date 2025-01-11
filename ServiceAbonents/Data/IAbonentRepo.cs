﻿using Azure;
using Microsoft.AspNetCore.JsonPatch;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Data
{
    public interface IAbonentRepo
    {
        bool SaveChange();
        IEnumerable<Abonent> GetAllAbonents(FilterDto filter);
        Abonent GetAbonentById(Guid id);
        Abonent CreateAbonent(AbonentCreateDto abonent, Guid TemporaryId);
        void Update(Guid id, AbonentsUpdateDto updateAbonent);
        void UpdateNewAbonent(Guid id, UpdateNewAbonentDto newAbonent);
        void CreateSimilarAbonent(TransferDataAbonentDto newAbonent);
        void GetAbonentByPhoneNumber(string PhoneNumber);
    }
}

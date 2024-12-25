using Microsoft.AspNetCore.Http.HttpResults;
using ServiceAbonents.Debiting;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;
using ServiceAbonents.RabbitMq;

namespace ServiceAbonents.Data
{
    public class AbonentRepo : IAbonentRepo
    {
        private readonly AppDbContext _context;
        //private readonly ISender _send;
        public AbonentRepo(AppDbContext context)
        {
            _context = context;
            //_send = send;
        }

        public void CreateAbonent(Abonent abonent)
        {
            if (abonent == null)
                throw new ArgumentNullException(nameof(abonent));

            //_send.SendMessage(new TransactionDto { Amount = 1, AbonentId = 1, OperationType = "1", PaymentMethod = "1" });
            _context.Abonents.Add(abonent);
            Debiting.Debiting.AddNewAbonent(abonent);
            //_send.SendMessage(abonent);
        }

        public Abonent GetAbonentById(int id)
        {
            var abonent = _context.Abonents.FirstOrDefault(x => x.Id == id);

            return _context.Abonents.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Abonent> GetAllAbonents()
        {
            return _context.Abonents.ToList();
        }

        public int GetTariffIdByAbonentId(int id)
        {
            return GetAbonentById(id).TarrifId;
        }

        public void Update(int id, AbonentsUpdateDto updateAbonent)
        {
            if (updateAbonent == null)
                throw new ArgumentNullException(nameof(updateAbonent));

            var abonent = GetAbonentById(id);

            if (abonent != null)
            {
                if (updateAbonent.TarrifId != 0)
                    abonent.TarrifId = updateAbonent.TarrifId;

                if (updateAbonent.Name != string.Empty)
                    abonent.Name = updateAbonent.Name;

                if (updateAbonent.Surname != string.Empty)
                    abonent.Surname = updateAbonent.Surname;

                if (updateAbonent.Patronymic != string.Empty)
                    abonent.Patronymic = updateAbonent.Patronymic;

                if (updateAbonent.PhoneNumber != string.Empty)
                    abonent.PhoneNumber = updateAbonent.PhoneNumber;

                if (updateAbonent.PasportData != string.Empty)
                    abonent.PasportData = updateAbonent.PasportData;

                if (updateAbonent.Balance != 0)
                    abonent.Balance = updateAbonent.Balance;

                //if (updateAbonent.DateForDeduct != string.Empty)
                //    abonent.DateForDeduct = DateTime.Now.ToString("dd.MM.yyyy");

                _context.Update(updateAbonent);
                _context.SaveChanges();
            }
        }

        public bool SaveChange()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
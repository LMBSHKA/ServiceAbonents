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
        private readonly IDebiting _debiting;

        public AbonentRepo(AppDbContext context, IDebiting debiting)
        {
            _context = context;
            _debiting = debiting;
        }

        public void CreateAbonent(User abonent)
        {
            if (abonent == null)
                throw new ArgumentNullException(nameof(abonent));

            _context.Abonents.Add(abonent);
            SaveChange();
            _debiting.AddNewAbonent(new DebitingAbonentDto {
                Id = abonent.Id,
                TarrifId = abonent.TarrifId,
                Balance = abonent.Balance
            });
        }

        public User GetAbonentById(int id)
        {
            var abonent = _context.Abonents.FirstOrDefault(x => x.Id == id);

            return _context.Abonents.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<User> GetAllAbonents()
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

                if (updateAbonent.DateForDeduct != string.Empty)
                    abonent.DateForDeduct = updateAbonent.DateForDeduct;

                _context.Update(abonent);
                _context.SaveChanges();
            }
        }

        public bool SaveChange()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
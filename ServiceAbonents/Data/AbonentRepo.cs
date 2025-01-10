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
        private readonly ISender _sender;
        private readonly IRemainRepo _remain;

        public AbonentRepo(AppDbContext context, IDebiting debiting, ISender sender, IRemainRepo remain)
        {
            _context = context;
            _debiting = debiting;
            _sender = sender;
            _remain = remain;
        }

        public Abonent CreateAbonent(AbonentCreateDto newAbonent)
        {
            if (newAbonent == null)
                throw new ArgumentNullException(nameof(newAbonent));

            var abonent = new Abonent
            {
                Id = 0,
                Name = newAbonent.Name,
                Surname = newAbonent.Surname,
                Patronymic = newAbonent.Patronymic,
                PasportData = newAbonent.PasportData,
            };

            _context.Abonents.Add(abonent);
            SaveChange();
            _sender.SendMessage(abonent.Id);
            //_debiting.AddNewAbonent(new DebitingAbonentDto {
            //    Id = abonent.Id,
            //    TarrifId = abonent.TarrifId,
            //    Balance = abonent.Balance
            //});

            return abonent;
        }

        public void CreateSimilarAbonent(TransferDataAbonentDto update)
        {
            var abonent = GetAbonentById(update.AbonentId);
            var newAbonent = new Abonent
            {
                Id = 0,
                TarrifId = update.TarifId,
                PhoneNumber = update.PhoneNumber,
                Name = abonent.Name,
                Surname = abonent.Surname,
                Patronymic = abonent.Patronymic,
                PasportData = abonent.PasportData,
            };

            _context.Add(newAbonent);
            _context.SaveChanges();
        }

        public Abonent GetAbonentById(int id)
        {
            var abonent = _context.Abonents.FirstOrDefault(x => x.Id == id);

            return _context.Abonents.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Abonent> GetAllAbonents()
        {
            return _context.Abonents.OrderBy(x => x.Id).ToList();
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
                //if (updateAbonent.TarrifId != 0)
                //{
                //    abonent.TarifCost = newTarifCost;
                //    abonent.TarrifId = updateAbonent.TarrifId;

                //    if (abonent.Balance >= costWithDiscoint)
                //        _debiting.DebitingSwitchedTarif(abonent.Id, costWithDiscoint, abonent.Balance);
                //    else
                //        _debiting.AddOldAbonent(new DebitingAbonentDto
                //        {
                //            Id = abonent.Id,
                //            TarifCost = abonent.TarifCost,
                //            Balance = abonent.Balance,
                //            TarrifId = abonent.Id
                //        });

                //    Console.WriteLine($"Скидка: {costWithDiscoint}");
                //}

                if (updateAbonent.Name != string.Empty)
                    abonent.Name = updateAbonent.Name;

                if (updateAbonent.Surname != string.Empty)
                    abonent.Surname = updateAbonent.Surname;

                if (updateAbonent.Patronymic != string.Empty)
                    abonent.Patronymic = updateAbonent.Patronymic;

                if (updateAbonent.PasportData != string.Empty)
                    abonent.PasportData = updateAbonent.PasportData;

                _context.Update(abonent);
                _context.SaveChanges();
            }
        }

        public void UpdateNewAbonent(int id, UpdateNewAbonentDto newAbonent)
        {
            if (newAbonent == null)
                throw new ArgumentNullException(nameof(newAbonent));

            var abonent = GetAbonentById(id);

            abonent.TarrifId = newAbonent.TarifId;
            abonent.PhoneNumber = newAbonent.PhoneNumber;
            abonent.TarifCost = newAbonent.TarifCost;

            _context.Update(abonent);
            _context.SaveChanges();
        }

        public bool SaveChange()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
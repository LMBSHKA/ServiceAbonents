using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
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

        public Abonent CreateAbonent(AbonentCreateDto newAbonent, Guid temporaryId)
        {
            if (newAbonent == null)
                throw new ArgumentNullException(nameof(newAbonent));

            var abonent = new Abonent
            {
                Id = new Guid(),
                Name = newAbonent.Name,
                Surname = newAbonent.Surname,
                Patronymic = newAbonent.Patronymic,
                Pasport = newAbonent.Pasport,
                PhoneNumber = newAbonent.PhoneNumber
            };

            _context.Abonents.Add(abonent);
            SaveChange();
            _sender.SendMessage(new IdForCartDto { AbonentId = abonent.Id, TemporaryId = temporaryId });

            return abonent;
        }

        public void CreateSimilarAbonent(TransferDataAbonentDto update)
        {
            var abonent = GetAbonentById(update.AbonentId);
            var newAbonent = new Abonent
            {
                Id = new Guid(),
                TariffId = update.TariffId,
                PhoneNumber = update.PhoneNumber,
                Name = abonent.Name,
                Surname = abonent.Surname,
                Patronymic = abonent.Patronymic,
                Pasport = abonent.Pasport,
                TariffCost = update.TariffCost
            };

            _context.Add(newAbonent);
            _context.SaveChanges();

            _debiting.AddNewAbonent(new DebitingAbonentDto
            {
                Id = newAbonent.Id,
                TariffId = newAbonent.TariffId,
                Balance = newAbonent.Balance,
                TariffCost = newAbonent.TariffCost
            });

            if (update.TariffId.Length > 2)
            {
                var splitTariff = update.TariffId.Split('-');
                var remain = new Remain {
                    ClientId = newAbonent.Id,
                    RemainGb = short.Parse(splitTariff[0]),
                    RemainMin = short.Parse(splitTariff[1]),
                    RemainSMS = short.Parse(splitTariff[2]),
                    UnlimVideo = bool.Parse(splitTariff[3]),
                    UnlimSocials = bool.Parse(splitTariff[4]),
                    UnlimMusic = bool.Parse(splitTariff[5]),
                    LongDistanceCall = bool.Parse(splitTariff[6])
                };

                _remain.CreateRemain(remain);
            }

            else
                _sender.SendMessage(new IdForTarifDto { AbonentId = newAbonent.Id, TariffId = newAbonent.TariffId });
        }

        public Abonent GetAbonentById(Guid id)
        {
            return _context.Abonents.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Abonent> GetAllAbonents(FilterDto filter)
        {
            var abonets = _context.Abonents.Where(p =>
            EF.Functions.Like(p.Name!, $"%{filter.Name}%") &
            EF.Functions.Like(p.Surname!, $"%{filter.Surname}%") &
            EF.Functions.Like(p.PhoneNumber!, $"%{filter.PhoneNumber}%") &
            EF.Functions.Like(Convert.ToString(p.TariffId)!, $"%{filter.TariffId}%") &
            EF.Functions.Like(p.Patronymic, $"%{filter.Patronymic}%"));

            return abonets.OrderBy(x => x.Id).ToList();
        }

        public void Update(Guid id, AbonentsUpdateDto updateAbonent)
        {
            if (updateAbonent == null)
                throw new ArgumentNullException(nameof(updateAbonent));

            var abonent = GetAbonentById(id);

            if (abonent != null)
            {
                if (updateAbonent.Name != string.Empty)
                    abonent.Name = updateAbonent.Name;

                if (updateAbonent.Surname != string.Empty)
                    abonent.Surname = updateAbonent.Surname;

                if (updateAbonent.Patronymic != string.Empty)
                    abonent.Patronymic = updateAbonent.Patronymic;

                if (updateAbonent.Pasport != string.Empty)
                    abonent.Pasport = updateAbonent.Pasport;

                if (updateAbonent.TariffCost != 0)
                    abonent.TariffCost = updateAbonent.TariffCost;

                if (updateAbonent.Status != null)
                    abonent.Status = (bool)updateAbonent.Status;

                _context.Update(abonent);
                _context.SaveChanges();
            }
        }

        public void UpdateNewAbonent(Guid id, UpdateNewAbonentDto newAbonent)
        {
            if (newAbonent == null)
                throw new ArgumentNullException(nameof(newAbonent));

            var abonent = GetAbonentById(id);

            abonent.TariffId = newAbonent.TariffId;
            abonent.PhoneNumber = newAbonent.PhoneNumber;
            abonent.TariffCost = newAbonent.TariffCost;

            if (newAbonent.TariffId.Length > 2)
            {
                var splitTarif = newAbonent.TariffId.Split('-');
                var newRemain = new RemainUpdateDto {
                    RemainGb = short.Parse(splitTarif[0]),
                    RemainMin = short.Parse(splitTarif[1]),
                    RemainSMS = short.Parse(splitTarif[2]),
                    UnlimVideo = bool.Parse(splitTarif[3]),
                    UnlimSocials = bool.Parse(splitTarif[4]),
                    UnlimMusic = bool.Parse(splitTarif[5]),
                    LongDistanceCall = bool.Parse(splitTarif[6])
                };
                _remain.Update(abonent.Id, newRemain);
            }

            _context.Update(abonent);
            _context.SaveChanges();

            _debiting.AddNewAbonent(new DebitingAbonentDto { 
                Id = abonent.Id,
                TariffId = newAbonent.TariffId,
                Balance = abonent.Balance,
                TariffCost = newAbonent.TariffCost
            });
        }

        public bool SaveChange()
        {
            return _context.SaveChanges() >= 0;
        }

        public TransferForAuthDto GetAbonentByPhoneNumber(string phoneNumber)
        { 
            
            var abonent = _context.Abonents.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
            if (abonent == null)
            {
                throw new KeyNotFoundException("Пользователь не найден");
            }
            return new TransferForAuthDto
            {
                AbonentId = abonent.Id.ToString(),
                PhoneNumber = abonent.PhoneNumber,
                Role = abonent.Role
            };
        }
    }
}
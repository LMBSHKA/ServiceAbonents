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
                PasportData = newAbonent.PasportData,
            };

            _context.Abonents.Update(abonent);
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
                TarrifId = update.TarifId,
                PhoneNumber = update.PhoneNumber,
                Name = abonent.Name,
                Surname = abonent.Surname,
                Patronymic = abonent.Patronymic,
                PasportData = abonent.PasportData,
                TarifCost = update.TarifCost
            };

            _context.Add(newAbonent);
            _context.SaveChanges();

            if (update.TarifId.Length > 2)
            {
                var splitTarif = update.TarifId.Split('-');
                var remain = new Remain {
                    ClientId = newAbonent.Id,
                    RemainGb = short.Parse(splitTarif[0]),
                    RemainMin = short.Parse(splitTarif[1]),
                    RemainSMS = short.Parse(splitTarif[2]),
                    UnlimVideo = bool.Parse(splitTarif[3]),
                    UnlimSocials = bool.Parse(splitTarif[4]),
                    UnlimMusic = bool.Parse(splitTarif[5]),
                    LongDistanceCall = bool.Parse(splitTarif[6])
                };

                _remain.CreateRemain(remain);
            }

            else
                _sender.SendMessage(new IdForTarifDto { AbonentId = newAbonent.Id, TarifId = newAbonent.TarrifId });
        }

        public Abonent GetAbonentById(Guid id)
        {
            return _context.Abonents.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Abonent> GetAllAbonents(FilterDto filter)
        {
            var abonets = _context.Abonents.Where(p =>
            EF.Functions.Like(p.Name!, $"%{filter.Name}%") ||
            EF.Functions.Like(p.Surname!, $"%{filter.Surname}%") ||
            EF.Functions.Like(p.PhoneNumber!, $"%{filter.PhoneNumber}%") ||
            EF.Functions.Like(Convert.ToString(p.TarrifId)!, $"%{filter.TarifId}%") ||
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

                if (updateAbonent.PasportData != string.Empty)
                    abonent.PasportData = updateAbonent.PasportData;

                _context.Update(abonent);
                _context.SaveChanges();
            }
        }

        public void UpdateNewAbonent(Guid id, UpdateNewAbonentDto newAbonent)
        {
            if (newAbonent == null)
                throw new ArgumentNullException(nameof(newAbonent));

            var abonent = GetAbonentById(id);

            abonent.TarrifId = newAbonent.TarifId;
            abonent.PhoneNumber = newAbonent.PhoneNumber;
            abonent.TarifCost = newAbonent.TarifCost;

            if (newAbonent.TarifId.Length > 2)
            {
                var splitTarif = newAbonent.TarifId.Split('-');
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
        }

        public bool SaveChange()
        {
            return _context.SaveChanges() >= 0;
        }

        public void GetAbonentByPhoneNumber(string phoneNumber)
        {
            var abonent = _context.Abonents.FirstOrDefault(x => x.PhoneNumber == phoneNumber);

            _sender.SendMessage(new TransferForAuthDto 
            {
                AbonentId = abonent.Id,
                PhoneNumber = abonent.PhoneNumber,
                Role = abonent.Role
            });
        }
    }
}
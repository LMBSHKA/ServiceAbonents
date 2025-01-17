using MassTransit;
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
        private readonly IRemainRepo _remain;
        private readonly IRequestClient<IdForCartDto> _client;

        public AbonentRepo(AppDbContext context, IDebiting debiting, IRemainRepo remain, IRequestClient<IdForCartDto> client)
        {
            _client = client;
            _context = context;
            _debiting = debiting;
            _remain = remain;
        }

        public async Task CreateAbonent(AbonentCreateDto newAbonent, Guid temporaryId)
        {
            if (newAbonent == null)
                throw new ArgumentNullException(nameof(newAbonent));

            var response = await _client.GetResponse<ListTransferDataAbonentDto>(
                new IdForCartDto { TemporaryId = temporaryId });

            var dataAbonents = response.Message.listData;

            foreach (var dataAbonent in dataAbonents)
            {
                var abonent = new Abonent
                {
                    Id = Guid.NewGuid(),
                    Name = newAbonent.Name,
                    Surname = newAbonent.Surname,
                    Patronymic = newAbonent.Patronymic,
                    Pasport = newAbonent.Pasport,
                    PhoneNumber = dataAbonent.PhoneNumber,
                    TariffId = dataAbonent.TariffId,
                    TariffCost = dataAbonent.TariffCost,
                    TariffName = dataAbonent.dataTariff.TariffName
                };
                await _context.Abonents.AddAsync(abonent);
                

                _debiting.AddNewAbonent(new DebitingAbonentDto
                {
                    Id = abonent.Id,
                    TariffCost = abonent.TariffCost,
                    Balance = abonent.Balance
                });

                var remain = new Remain
                {
                    ClientId = abonent.Id,
                    RemainGb = dataAbonent.dataTariff.RemainGb,
                    RemainMin = dataAbonent.dataTariff.RemainMin,
                    RemainSMS = dataAbonent.dataTariff.RemainSMS,
                    UnlimMusic = dataAbonent.dataTariff.UnlimMusic,
                    UnlimSocials = dataAbonent.dataTariff.UnlimSocials,
                    UnlimVideo = dataAbonent.dataTariff.UnlimVideo,
                    LongDistanceCall = dataAbonent.dataTariff.LongDistanceCall
                };

                await _context.Remains.AddAsync(remain); ;
            }
            await _context.SaveChangesAsync();
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

            return abonets.OrderBy(x => x.Surname).ToList();
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
using Microsoft.EntityFrameworkCore;
using ServiceAbonents.Debiting;
using ServiceAbonents.Dtos;
using ServiceAbonents.RabbitMq;

namespace ServiceAbonents.Data
{
    public class SwitchTarif : ISwitchTarif
    {
        private readonly AppDbContext _context;
        private readonly IUpdateBalance _updateBalance;
        private readonly ISender _send;
        private readonly IRemainRepo _remain;
        private readonly IDebiting _debiting;

        public SwitchTarif(AppDbContext context, IUpdateBalance updateBalance, ISender send,
            IRemainRepo remain, IDebiting debiting)
        {
            _debiting = debiting;
            _context = context;
            _updateBalance = updateBalance;
            _send = send;
            _remain = remain;
        }

        public void UpdateTarif(SwitchTarifDto newTarif, Guid AbonentId)
        {
            if (newTarif == null)
                throw new ArgumentNullException(nameof(newTarif));

            var abonent = _context.Abonents.FirstOrDefault(x => x.Id.Equals(AbonentId));

            if (abonent != null)
            {
                if (!newTarif.Tariff.IsNullOrEmpty())
                {
                    decimal newTarifCost = 500;
                    abonent.TariffCost = newTarifCost;
                    abonent.TariffId = newTarif.Tariff;
                    _context.Update(abonent);
                    _context.SaveChanges();

                    var costWithDiscoint = newTarifCost - GetDiscount(abonent.Id, newTarifCost);

                    if (abonent.Balance >= costWithDiscoint)
                        DebitingSwitchedTarif(abonent.Id, costWithDiscoint);

                    else
                        _debiting.AddOldAbonent(new DebitingAbonentDto
                        {
                            Id = abonent.Id,
                            TariffCost = newTarifCost,
                            Balance = abonent.Balance,
                            TariffId = newTarif.Tariff
                        });
                }
            }
        }

        private decimal GetDiscount(Guid abonentId, decimal newTarifCost)
        {
            var remain = _remain.GetRemainByAbonentId(abonentId);
            decimal discount = remain.RemainGb + remain.RemainMin * (decimal)0.05 + remain.RemainSMS * (decimal)0.7;

            if (discount > newTarifCost * (decimal)0.5)
                return newTarifCost * (decimal)0.5;

            return discount;
        }

        public void DebitingSwitchedTarif(Guid abonentId, decimal tarifCost)
        {
            _updateBalance.TopUpAndDebitingBalance(new TopUpDto
            {
                ClientId = abonentId,
                Amount = -tarifCost
            });
            
            _send.SendMessage(_debiting.SetTransaction(abonentId, tarifCost));
            UpdateDate(abonentId);
            var abonent = _context.Abonents.FirstOrDefault(x => x.Id.Equals(abonentId));
            Console.WriteLine("Денег хваатает на балансе");

        }

        public void UpdateDate(Guid abonentId)
        {

            var abonent = _context.Abonents.FirstOrDefault(x => x.Id.Equals(abonentId));
            _context.Entry(abonent).Reload();
            abonent.DateForDeduct = DateTime.Now.AddMonths(1).ToString("dd.MM.yyyy");
            abonent.Status = true;
            _context.Update(abonent);
            _context.SaveChanges();
        }
    }
}
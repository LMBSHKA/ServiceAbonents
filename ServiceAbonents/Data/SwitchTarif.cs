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

        public void UpdateTarif(SwitchTarifDto newTarif, int AbonentId)
        {
            if (newTarif == null)
                throw new ArgumentNullException(nameof(newTarif));

            var abonent = _context.Abonents.FirstOrDefault(x => x.Id == AbonentId);

            if (abonent != null)
            {
                if (newTarif.Tarif != 0)
                {
                    decimal newTarifCost = 500;
                    abonent.TarifCost = newTarifCost;
                    abonent.TarrifId = newTarif.Tarif;
                    _context.Update(abonent);
                    _context.SaveChanges();

                    var costWithDiscoint = newTarifCost - GetDiscount(abonent.Id, newTarifCost);

                    if (abonent.Balance >= costWithDiscoint)
                        DebitingSwitchedTarif(abonent.Id, costWithDiscoint);

                    else
                        _debiting.AddOldAbonent(new DebitingAbonentDto
                        {
                            Id = abonent.Id,
                            TarifCost = newTarifCost,
                            Balance = abonent.Balance,
                            TarrifId = newTarif.Tarif
                        });
                }
            }
        }

        private decimal GetDiscount(int abonentId, decimal newTarifCost)
        {
            var remain = _remain.GetRemainByAbonentId(abonentId);
            decimal discount = remain.ReaminGb + remain.RemainMin * (decimal)0.05 + remain.RemainSMS * (decimal)0.7;

            if (discount > newTarifCost * (decimal)0.5)
                return newTarifCost * (decimal)0.5;

            return discount;
        }

        public void DebitingSwitchedTarif(int abonentId, decimal tarifCost)
        {
            _updateBalance.TopUpAndDebitingBalance(new TopUpDto
            {
                ClientId = abonentId,
                Amount = -tarifCost
            });
            
            _send.SendMessage(_debiting.SetTransaction(abonentId, tarifCost));
            UpdateDate(abonentId);
            var abonent = _context.Abonents.FirstOrDefault(x => x.Id == abonentId);
            Console.WriteLine("Денег хваатает на балансе");

        }

        public void UpdateDate(int abonentId)
        {

            var abonent = _context.Abonents.FirstOrDefault(x => x.Id == abonentId);
            _context.Entry(abonent).Reload();
            abonent.DateForDeduct = DateTime.Now.AddMonths(1).ToString("dd.MM.yyyy");
            abonent.Status = true;
            _context.Update(abonent);
            _context.SaveChanges();
        }
    }
}
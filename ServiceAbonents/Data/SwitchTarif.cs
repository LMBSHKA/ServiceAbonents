﻿using Microsoft.EntityFrameworkCore;
using ServiceAbonents.Debiting;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;
using ServiceAbonents.RabbitMq;
using System.Data;

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

        public void UpdateTarif(SwitchTarifDto dynamicTarif, Guid abonentId, string staticTariff, decimal tariffCost)
        {
            var abonent = _context.Abonents.FirstOrDefault(x => x.Id == abonentId);

            if (abonent != null)
            {
                var tariffId = staticTariff;
                var costWithDiscount = tariffCost - GetDiscount(abonent.Id, tariffCost);

                if (dynamicTarif.Minutes != null)
                    tariffId = $"" +
                        $"{dynamicTarif.Gigabytes}-" +
                        $"{dynamicTarif.Minutes}-" +
                        $"{dynamicTarif.Sms}-" +
                        $"{dynamicTarif.UnlimVideo}-" +
                        $"{dynamicTarif.UnlimSocials}-" +
                        $"{dynamicTarif.UnlimMusic}-" +
                        $"{dynamicTarif.LongDistanceCall}";

                abonent.TariffId = tariffId;
                abonent.TariffCost = tariffCost;

                _context.Update(abonent);
                _context.SaveChanges();

                if (abonent.Balance >= costWithDiscount)
                    DebitingSwitchedTarif(abonent.Id, costWithDiscount);

                else
                    _debiting.AddOldAbonent(new DebitingAbonentDto
                    {
                        Id = abonent.Id,
                        TariffCost = tariffCost,
                        Balance = abonent.Balance,
                        TariffId = tariffId
                    });
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
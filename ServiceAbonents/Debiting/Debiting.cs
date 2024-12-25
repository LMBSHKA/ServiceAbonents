using ServiceAbonents.Data;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;
using ServiceAbonents.RabbitMq;
using System.Reflection.Metadata.Ecma335;

namespace ServiceAbonents.Debiting
{
    public class Debiting : IDebiting
    {
        private static List<Abonent> newAbonents = new List<Abonent>();
        private readonly IUpdateBalance _updateBalance;
        private readonly ISender _send;

        public Debiting(ISender send, IUpdateBalance updateBalance)
        {
            _updateBalance = updateBalance;
            _send = send;
        }

        public Abonent FindAbonents(int id) => newAbonents.FirstOrDefault(x => x.Id == id);

        public void AddNewAbonent(Abonent abonent) => newAbonents.Add(abonent);
        
        public void RemoveAbonent(Abonent abonent) => newAbonents.Remove(abonent);
        

        public void Update(Abonent abonent)
        {
            var newAbonent = FindAbonents(abonent.Id);
            RemoveAbonent(newAbonent);
            AddNewAbonent(abonent);
        }

        public bool ExamTransaction(TopUpDto newBalance)
        {

            var tariffCost = 500;
            var newAbonent = FindAbonents(newBalance.ClientId);

            if (newAbonent != null && newAbonent.Balance > 0 && newAbonent.Balance + newBalance.Amount - tariffCost >= 0)
            {
                RemoveAbonent(newAbonent);
                _send.SendMessage(SetTransaction(newBalance.ClientId, tariffCost));
                return _updateBalance.TopUpAndDebitingBalance(new TopUpDto
                {
                    ClientId = newBalance.ClientId,
                    Amount = newBalance.Amount - tariffCost
                });
            }

            if (newBalance.Amount < 0 || (newAbonent != null && newBalance.Amount < tariffCost) || newAbonent == null)
            {
                var result = _updateBalance.TopUpAndDebitingBalance(newBalance);
                if (result == true && newAbonent != null)
                {
                    newAbonent.Balance += newBalance.Amount;
                    Update(newAbonent);
                }
                return result;
            }

            _send.SendMessage(SetTransaction(newBalance.ClientId, newBalance.Amount));
            RemoveAbonent(newAbonent);
            return true;
        }

        private TransactionDto SetTransaction(int id, decimal amount)
        {
            return new TransactionDto
            {
                ClientId = id,
                Amount = amount,
                PaymentMethod = "Balance",
                OperationType = "Mothly transaction"
            };
        }
    }
}

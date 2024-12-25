using ServiceAbonents.Data;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;
using ServiceAbonents.RabbitMq;

namespace ServiceAbonents.Debiting
{
    public class Debiting
    {
        private static List<Abonent> newAbonents = new List<Abonent>();
        public static Abonent FindAbonents(int id)
        {
            var abonent = newAbonents.FirstOrDefault(x => x.Id == id);
            return abonent;
        }

        public static void AddNewAbonent(Abonent abonent) => newAbonents.Add(abonent);
        
        public static void RemoveAbonent(Abonent abonent)
        {
            newAbonents.Remove(abonent);
        }

        public static void Update(Abonent abonent)
        {
            var newAbonent = FindAbonents(abonent.Id);
            RemoveAbonent(newAbonent);
            AddNewAbonent(abonent);
        }

        public static bool ExamTransaction(TopUpDto newBalance)
        {

            var tariffCost = 500;
            var newAbonent = FindAbonents(newBalance.ClientId);

            if (newAbonent != null && newAbonent.Balance > 0 && newAbonent.Balance + newBalance.Amount - tariffCost >= 0)
            {
                RemoveAbonent(newAbonent);
                RabbitMqSender.SendMessage(SetTransaction(newBalance.ClientId, tariffCost));

                return UpdateBalance.TopUpAndDebitingBalance(new TopUpDto
                {
                    ClientId = newBalance.ClientId,
                    Amount = newBalance.Amount - tariffCost
                });
            }

            if (newBalance.Amount < 0 || (newAbonent != null && newBalance.Amount < tariffCost) || newAbonent == null)
            {
                return UpdateBalance.TopUpAndDebitingBalance(newBalance);
            }

            RabbitMqSender.SendMessage(SetTransaction(newBalance.ClientId, newBalance.Amount));
            RemoveAbonent(newAbonent);
            return true;
        }

        public static TransactionDto SetTransaction(int id, decimal amount)
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

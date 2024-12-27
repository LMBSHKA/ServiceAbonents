using ServiceAbonents.Data;
using ServiceAbonents.Dtos;
using ServiceAbonents.RabbitMq;

namespace ServiceAbonents.Debiting
{
    public class Debiting : IDebiting
    {
        private static List<DebitingAbonentDto> _newAbonents = new List<DebitingAbonentDto>();
        private static List<DebitingAbonentDto> _oldAbonents = new List<DebitingAbonentDto>();
        private readonly IUpdateBalance _updateBalance;
        private readonly ISender _send;
        private readonly IServiceScopeFactory _scopeFactory;

        public Debiting(ISender send, IUpdateBalance updateBalance, IServiceScopeFactory scopeFactory)
        {
            _updateBalance = updateBalance;
            _send = send;
            _scopeFactory = scopeFactory;
        }

        public DebitingAbonentDto FindNewAbonent(int id) => _newAbonents.FirstOrDefault(x => x.Id == id);

        public DebitingAbonentDto FindOldAbonent(int id) => _oldAbonents.FirstOrDefault(x => x.Id == id);

        public void AddNewAbonent(DebitingAbonentDto abonent) => _newAbonents.Add(abonent);

        public void AddOldAbonent(DebitingAbonentDto abonent) => _oldAbonents.Add(abonent);

        public void UpdateOldAbonent(DebitingAbonentDto abonent)
        {
            var oldAbonent = FindOldAbonent(abonent.Id);
            _oldAbonents.Remove(oldAbonent);
            AddOldAbonent(abonent);
        }

        public void UpdateNewAbonent(DebitingAbonentDto abonent)
        {
            var newAbonent = FindNewAbonent(abonent.Id);
            _newAbonents.Remove(newAbonent);
            AddNewAbonent(abonent);
        }

        public bool ExamTransaction(TopUpDto newBalance)
        {
            var tariffCost = 500;
            var newAbonent = FindNewAbonent(newBalance.ClientId);

            if (newAbonent == null)
            {
                var oldAbonent = FindOldAbonent(newBalance.ClientId);
                if (oldAbonent != null)
                    return DebitingOldAbonents(oldAbonent, tariffCost, newBalance);
            }

            if (newAbonent != null && newAbonent.Balance > 0 && 
                newAbonent.Balance + newBalance.Amount - tariffCost >= 0)
                return BalanceMoreThenZero(newAbonent, newBalance, tariffCost);

            if (newBalance.Amount < 0 || (newAbonent != null && newBalance.Amount < tariffCost) ||
                newAbonent == null)
                return TopUpOrDebiting(newAbonent, newBalance);

            return SingleTransactionForDebiting(newAbonent, tariffCost, newBalance);
        }

        private bool SingleTransactionForDebiting(DebitingAbonentDto newAbonent, 
            int tariffCost, TopUpDto newBalance)
        {
            _updateBalance.TopUpAndDebitingBalance(new TopUpDto
            {
                ClientId = newAbonent.Id,
                Amount = newBalance.Amount - tariffCost
            });

            UpdateDate(newBalance.ClientId);
            _newAbonents.Remove(newAbonent);

            return _send.SendMessage(SetTransaction(newBalance.ClientId, tariffCost)).Result;
        }

        private bool BalanceMoreThenZero(DebitingAbonentDto newAbonent, TopUpDto newBalance, int tariffCost)
        {
            _newAbonents.Remove(newAbonent);
            _send.SendMessage(SetTransaction(newBalance.ClientId, tariffCost));
            UpdateDate(newBalance.ClientId);

            return _updateBalance.TopUpAndDebitingBalance(new TopUpDto
            {
                ClientId = newBalance.ClientId,
                Amount = newBalance.Amount - tariffCost
            });
        }

        private bool TopUpOrDebiting(DebitingAbonentDto newAbonent, TopUpDto newBalance)
        {
            var result = _updateBalance.TopUpAndDebitingBalance(newBalance);

            if (result == true && newAbonent != null)
            {
                newAbonent.Balance += newBalance.Amount;
                UpdateNewAbonent(newAbonent);
            }
            return result;
        }

        private bool DebitingOldAbonents(DebitingAbonentDto oldAbonent, int tariffCost, TopUpDto newBalance)
        {
            if (newBalance.Amount + oldAbonent.Balance >= tariffCost)
            {
                _updateBalance.TopUpAndDebitingBalance(new TopUpDto
                {
                    ClientId = oldAbonent.Id,
                    Amount = newBalance.Amount - tariffCost
                });
                _oldAbonents.Remove(oldAbonent);
                _send.SendMessage(SetTransaction(newBalance.ClientId, tariffCost));
                UpdateDate(newBalance.ClientId);

                return true;
            }

            oldAbonent.Balance += newBalance.Amount;
            UpdateOldAbonent(oldAbonent);

            return _updateBalance.TopUpAndDebitingBalance(new TopUpDto
            {
                ClientId = oldAbonent.Id,
                Amount = newBalance.Amount
            });

        }

        public void UpdateDate(int id)
        {
            using (var scope = _scopeFactory.CreateAsyncScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IAbonentRepo>();
                repo.Update(id, new AbonentsUpdateDto
                {
                    DateForDeduct = DateTime.Now.AddMonths(1).ToString("dd.MM.yyyy ")
                });
            }
        }

        public TransactionDto SetTransaction(int id, decimal amount)
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
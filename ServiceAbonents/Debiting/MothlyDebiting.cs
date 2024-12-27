using Quartz;
using ServiceAbonents.Data;
using ServiceAbonents.RabbitMq;

namespace ServiceAbonents.Debiting
{
    public class MothlyDebiting : IJob
    {
        private readonly IAbonentRepo _abonentRepo;
        private readonly IDebiting _debiting;
        private readonly IUpdateBalance _updateBalance;
        private readonly ISender _sender;

        public MothlyDebiting(IAbonentRepo abonentRepo, IDebiting debiting, IUpdateBalance updateBalance, ISender sender)
        {
            _debiting = debiting;
            _abonentRepo = abonentRepo;
            _updateBalance = updateBalance;
            _sender = sender;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("work");
            var listAbonents = _abonentRepo.GetAllAbonents();
            

            foreach (var e in listAbonents)
            {
                if (e.DateForDeduct == DateTime.Now.ToString("dd.MM.yyyy"))
                {
                    var tarifCost = 500;

                    if (tarifCost > e.Balance)
                        _debiting.AddOldAbonent(new Dtos.DebitingAbonentDto
                        {
                            Id = e.Id,
                            Balance = e.Balance,
                            TarrifId = e.TarrifId
                        });

                    else
                    {
                        _updateBalance.TopUpAndDebitingBalance(new Dtos.TopUpDto
                        {
                            ClientId = e.Id,
                            Amount = -tarifCost
                        });
                        _sender.SendMessage(_debiting.SetTransaction(e.Id, tarifCost));
                        _debiting.UpdateDate(e.Id);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}

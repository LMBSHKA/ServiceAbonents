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
            var listAbonents = _abonentRepo.GetAllAbonents(new Dtos.FilterDto { Name = "", Surname = ""});
            

            foreach (var e in listAbonents)
            {
                if (e.DateForDeduct == DateTime.Now.ToString("dd.MM.yyyy"))
                {
                    if (e.TariffCost > e.Balance)
                        _debiting.AddOldAbonent(new Dtos.DebitingAbonentDto
                        {
                            Id = e.Id,
                            Balance = e.Balance,
                            TariffId = e.TariffId,
                            TariffCost = e.TariffCost
                        });

                    else
                    {
                        _updateBalance.TopUpAndDebitingBalance(new Dtos.TopUpDto
                        {
                            ClientId = e.Id,
                            Amount = -e.TariffCost
                        });
                        _sender.SendMessage(_debiting.SetTransaction(e.Id, e.TariffCost));
                        _debiting.UpdateDate(e.Id);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}

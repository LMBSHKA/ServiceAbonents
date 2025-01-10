using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using ServiceAbonents.Debiting;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;
using ServiceAbonents.RabbitMq;

namespace ServiceAbonents.Data
{
    public class UpdateBalance : IUpdateBalance
    {
        private static readonly string connectionString = "Server=tenuously-surprising-sawfish.data-1.use1.tembo.io;Port=5432;Database=AbonentsDb;User Id=postgres;Password=F5OXiaKwQc6V98WQ";

        public bool TopUpAndDebitingBalance(TopUpDto newBalance)
        {
            if (newBalance.Amount == 0)
                return true;

            using var con = new NpgsqlConnection(connectionString);
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(con)
                .Options;

            using var context = new AppDbContext(options);
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var abonent = context.Abonents.FirstOrDefault(x => x.Id == newBalance.ClientId);

                    abonent.Balance = abonent.Balance + newBalance.Amount;
                    context.Update(abonent);
                    context.SaveChanges();
                    transaction.Commit();
                    var abonent2 = context.Abonents.FirstOrDefault(x => x.Id == newBalance.ClientId);
                    return true;
                }

                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
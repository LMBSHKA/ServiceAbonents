using Microsoft.EntityFrameworkCore;
using Npgsql;
using ServiceAbonents.Dtos;

namespace ServiceAbonents.Data
{
    public class UpdateBalance
    {
        private static readonly string connectionString = "Server=localhost;Port=5432;Database=ServiceAbonents_AT;User Id=postgres;Password=admin";

        public static bool TopUpAndDebitingBalance(TopUpDto newBalance)
        {
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
                    context.SaveChanges();
                    transaction.Commit();
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
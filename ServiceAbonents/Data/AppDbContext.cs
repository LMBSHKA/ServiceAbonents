using Microsoft.EntityFrameworkCore;
using ServiceAbonents.Models;

namespace ServiceAbonents.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Abonent> Abonents { get; set; }
        public DbSet<Remain> Remains { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            //Database.EnsureCreated();
        }
    }
}

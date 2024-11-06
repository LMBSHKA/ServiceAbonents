using Microsoft.EntityFrameworkCore;
using ServiceAbonents.Models;

namespace ServiceAbonents.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Abonent> Abonents { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}

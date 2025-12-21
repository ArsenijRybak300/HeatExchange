using Microsoft.EntityFrameworkCore;
using HeatExchangeCalculator.Models;

namespace HeatExchangeCalculator.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Calculation> Calculations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=HeatExchangeCalculator.db");
            }
        }
    }
}
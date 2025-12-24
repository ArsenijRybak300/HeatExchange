using Microsoft.EntityFrameworkCore;
using HeatExchangeCalculator.Models;

namespace HeatExchangeCalculator.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Calculation> Calculations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Calculation>()
                .HasIndex(c => c.CreatedDate);
            
            // Настройка значений по умолчанию
            modelBuilder.Entity<Calculation>()
                .Property(c => c.CreatedDate)
                .HasDefaultValueSql("datetime('now')");
        }
    }
}
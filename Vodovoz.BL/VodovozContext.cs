using Microsoft.EntityFrameworkCore;
using Vodovoz.DAL.Model;

namespace Vodovoz.DAL {
    public class VodovozContext : DbContext {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=123";

        public VodovozContext() => Database.EnsureCreated();
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(ConnectionString);
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Employee>()
                .HasOne(u => u.Division)
                .WithOne(p => p.Boss)
                .HasForeignKey<Division>(d => d.BossId);
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Division> Divisions { get; set; }

        public DbSet<Orders> Orders { get; set; }
    }
}
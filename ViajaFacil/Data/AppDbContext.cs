using Microsoft.EntityFrameworkCore;
using ViajaFacil.Models.Users;
using ViajaFacil.Models.Destines;

namespace ViajaFacil.Data {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Destine> Destines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Se quiser manter a configuração manual:
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Destine>().HasKey(d => d.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}

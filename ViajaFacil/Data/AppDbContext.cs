using Microsoft.EntityFrameworkCore;
using ViajaFacil.Models.Users;
using ViajaFacil.Models.Destines;
using ViajaFacil.Models.Reserves;

namespace ViajaFacil.Data {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<DestineModel> Destines { get; set; }

        public DbSet<ReserveModel> Reserves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Se quiser manter a configuração manual:
            modelBuilder.Entity<UserModel>().HasKey(u => u.Id);
            modelBuilder.Entity<DestineModel>().HasKey(d => d.Id);
            modelBuilder.Entity<ReserveModel>().HasKey(d => d.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}

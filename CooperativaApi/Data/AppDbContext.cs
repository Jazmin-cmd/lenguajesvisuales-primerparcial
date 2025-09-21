using Microsoft.EntityFrameworkCore;
using CooperativaApi.Models;

namespace CooperativaApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Profesion> Profesiones => Set<Profesion>();
        public DbSet<Socio> Socios => Set<Socio>();
        public DbSet<Aportacion> Aportaciones => Set<Aportacion>();
        public DbSet<Prestamo> Prestamos => Set<Prestamo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aportacion>()
                .Property(a => a.Monto)
                .HasPrecision(18, 2); // 18 dígitos totales, 2 decimales

            modelBuilder.Entity<Prestamo>()
                .Property(p => p.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Prestamo>()
                .Property(p => p.Interes)
                .HasPrecision(5, 2); // ej. 999.99 máximo de interés
        }

    }
}

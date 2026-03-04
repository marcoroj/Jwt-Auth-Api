using JwtIdentityApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JwtIdentityApi.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RefreshTokenHistorial>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Usuario)
                      .WithMany()
                      .HasForeignKey(e => e.UsuarioId);

                entity.Property(e => e.Token).HasMaxLength(500);
                entity.Property(e => e.RefreshToken).HasMaxLength(200);
                entity.Property(e => e.FechCreacion).HasColumnType("datetime");
                entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");

                // Columna calculada en SQL Server
                entity.Property(e => e.EsActivo)
                      .HasComputedColumnSql(
                          "IIF(FechaExpiracion < GETDATE(), CONVERT(bit,0), CONVERT(bit,1))"
                      );
            });
        }


        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<RefreshTokenHistorial> RefreshTokensHistoriales { get; set; }
    }
}

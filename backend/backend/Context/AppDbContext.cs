using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Context
{
    // Herda de IdentityDbContext para reaproveitar as tabelas de usuários do ASP.NET Core Identity.
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Coleta> Coletas { get; set; }

        public DbSet<Motorista> Motoristas { get; set; }

        public DbSet<Veiculo> Veiculos { get; set; }

        public DbSet<Ocorrencia> Ocorrencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Número da coleta deve ser único
            modelBuilder.Entity<Coleta>()
                .HasIndex(c => c.NumeroSolicitacao)
                .IsUnique();

            // Cliente -> Coletas
            modelBuilder.Entity<Coleta>()
                .HasOne(c => c.Cliente)
                .WithMany(c => c.Coletas)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Motorista -> Coletas
            modelBuilder.Entity<Coleta>()
                .HasOne(c => c.Motorista)
                .WithMany(m => m.Coletas)
                .HasForeignKey(c => c.MotoristaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Veículo -> Coletas
            modelBuilder.Entity<Coleta>()
                .HasOne(c => c.Veiculo)
                .WithMany(v => v.Coletas)
                .HasForeignKey(c => c.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Coleta -> Ocorrências
            modelBuilder.Entity<Ocorrencia>()
                .HasOne(o => o.Coleta)
                .WithMany(c => c.Ocorrencias)
                .HasForeignKey(o => o.ColetaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

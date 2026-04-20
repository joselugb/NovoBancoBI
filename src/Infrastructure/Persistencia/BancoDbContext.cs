using Application.Interfaces;
using Domain.Entidades;
using Domain.Enumeradores;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Infrastructure.Persistencia;

public class BancoDbContext : DbContext, IBancoDbContext
{
    public BancoDbContext(DbContextOptions<BancoDbContext> options)
        : base(options) { }

    public DbSet<Cuenta> Cuentas => Set<Cuenta>();
    public DbSet<Transaccion> Transacciones => Set<Transaccion>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ========= PostgreSQL ENUMS =========
        modelBuilder.HasPostgresEnum<TipoCuenta>();
        modelBuilder.HasPostgresEnum<EstadosCuenta>();
        modelBuilder.HasPostgresEnum<TiposTransacciones>();
        modelBuilder.HasPostgresEnum<EstadoTransaccion>();

        // ========= Tabla Cuenta =========
        modelBuilder.Entity<Cuenta>(entity =>
        {
            entity.ToTable("cuenta");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.NumeroCuenta)
                  .HasColumnName("numero_cuenta")
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(e => e.Balance)
                  .HasColumnName("balance")
                  .HasPrecision(15, 2)
                  .IsRequired();

            entity.HasIndex(e => e.NumeroCuenta)
                  .IsUnique();
        });

        // ========= Tabla Transaccion =========
        modelBuilder.Entity<Transaccion>(entity =>
        {
            entity.ToTable("transacciones");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Monto)
                  .HasPrecision(15, 2)
                  .IsRequired();

            entity.HasIndex(e => e.Referencia)
                  .IsUnique();
        });
    }

}
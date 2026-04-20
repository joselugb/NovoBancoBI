using Application.Interfaces;
using Domain.Entidades;
using Domain.Enumeradores;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.Persistencia;

public class BancoDbContext : DbContext, IBancoDbContext
{
    public BancoDbContext(DbContextOptions<BancoDbContext> options)
        : base(options) { }

    public DbSet<Cuenta> Cuentas => Set<Cuenta>();
    public DbSet<Transaccion> Transacciones => Set<Transaccion>();

    public DbSet<Cliente> Clientes => Set<Cliente>();

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
            
            entity.Property(e => e.EstadosCuenta)
                  .HasColumnName("estado_cuenta")
                  .HasConversion<int>()
                  .IsRequired();
        });

        // ========= Tabla Transaccion =========
        modelBuilder.Entity<Transaccion>(entity =>
        {
            entity.ToTable("transacciones");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.IdCuentaOrigen)
                  .HasColumnName("id_cuenta_origen")
                  .IsRequired();

            entity.Property(e => e.IdCuentaDestino)
                  .HasColumnName("id_cuenta_destino")
                  .IsRequired();

            entity.Property(e => e.Monto)
                  .HasPrecision(15, 2)
                  .HasColumnName("monto")
                  .IsRequired();

            entity.Property(e => e.Referencia)
                  .HasColumnName("referencia")
                  .HasMaxLength(100)
                  .IsRequired();

            entity.HasIndex(e => e.Referencia)
                    .HasDatabaseName("IX_Transacciones_Referencia")
                  .IsUnique();
            
            entity.Property(e => e.TipoTransaccion)
                  .HasColumnName("tipo_transaccion")
                  .HasConversion<int>()
                  .IsRequired();

            entity.Property(e => e.EstadoTransaccion)
                  .HasColumnName("estado_transaccion")
                  .HasConversion<int>()
                  .IsRequired();

            entity.Property(e => e.Fecha)
                  .HasColumnName("fecha");
        });

        // ========= Tabla Cliente =========
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("clientes");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.NombreCompleto)
                  .HasColumnName("nombre_completo")
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(e => e.DocumentoIdentidad)
                  .HasColumnName("documento_identidad")
                  .HasMaxLength(20)
                  .IsRequired();
        });

    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await Database.RollbackTransactionAsync(cancellationToken);
    }

}
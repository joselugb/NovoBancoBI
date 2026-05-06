using Application.Interfaces;
using Domain.Entidades;
using Domain.Enumeradores;
using Microsoft.EntityFrameworkCore;

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

            entity.Property(e => e.Id).HasColumnName("Id");

            entity.Property(e => e.NumeroCuenta)
                  .HasColumnName("numero_cuenta")
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(e => e.IdCliente)
                .HasColumnName("id_cliente");

            entity.Property(e => e.Moneda)
                .HasColumnName("moneda")
                .HasMaxLength(3)
                .IsRequired();

            entity.Property(e => e.Balance)
                  .HasColumnName("balance")
                  .HasPrecision(15, 2)
                  .IsRequired();

            entity.HasIndex(e => e.NumeroCuenta)
                  .IsUnique()
                  .HasDatabaseName("IX_Cuentas_NumeroCuenta");

            entity.HasIndex(e => e.IdCliente)
                .HasDatabaseName("IX_Cuentas_IdCliente");

            entity.Property(e => e.EstadosCuenta)
                  .HasColumnName("estado_cuenta")
                  .HasConversion<int>()
                  .IsRequired();

            entity.Property(e => e.FechaCreacion)
                .HasColumnName("fecha_creacion");

            // entity.Property(e => e.RowVersion)
            //     .HasColumnName("RowVersion")
            //     .IsRowVersion();
            entity.Property<uint>("xmin")
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(e => e.Cliente)
                .WithMany(e => e.Cuentas)
                .HasForeignKey(e => e.IdCliente)
                .HasConstraintName("FK_Cuentas_Clientes");

            entity.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Cuentas_Saldo_NoNegativo", "balance >= 0");
            });
        });

        // ========= Tabla Transaccion =========
        modelBuilder.Entity<Transaccion>(entity =>
        {
            entity.ToTable("transacciones");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("Id");

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

            entity.HasIndex(e => new { e.IdCuentaOrigen, e.Fecha })
                .HasDatabaseName("IX_Transacciones_IdCuentaOrigen_Fecha");

            entity.HasOne(e => e.Cuenta)
                .WithMany()
                .HasForeignKey(e => e.IdCuentaOrigen)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Transacciones_Cuentas");
        });

        // ========= Tabla Cliente =========
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("clientes");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");

            entity.Property(e => e.NombreCompleto)
                  .HasColumnName("nombre_completo")
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(e => e.DocumentoIdentidad)
                  .HasColumnName("documento_identidad")
                  .HasMaxLength(20)
                  .IsRequired();
            entity.HasIndex(x => x.DocumentoIdentidad)
                .IsUnique()
                .HasDatabaseName("IX_Clientes_DocumentoIdentidad");
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
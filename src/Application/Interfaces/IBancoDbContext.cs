using Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

    public interface IBancoDbContext
    {
        DbSet<Cuenta> Cuentas { get; }
        DbSet<Transaccion> Transacciones { get; }
        DbSet<Cliente> Clientes { get; }
        /// <summary>
        /// Guarda todos los cambios pendientes.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Inicia una transacción en la base de datos.
        /// </summary>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Confirma (commit) la transacción actual en la base de datos.
        /// </summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Revierte (rollback) la transacción actual en la base de datos.
        /// </summary>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }

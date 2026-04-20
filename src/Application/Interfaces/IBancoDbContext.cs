using Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

    public interface IBancoDbContext
    {
        DbSet<Cuenta> Cuentas { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

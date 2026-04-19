using Application.Interfaces;
using Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia;

public class BancoDbContext : DbContext, IBancoDbContext
{
    public BancoDbContext(DbContextOptions<BancoDbContext> options)
        : base(options) { }

    public DbSet<Cuenta> Cuentas => Set<Cuenta>();
}
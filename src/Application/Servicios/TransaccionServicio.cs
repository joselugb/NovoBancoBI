using Application.DTOs;
using Domain.Entidades;
using Domain.Enumeradores;
using Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Application.Servicios;

public class TransaccionServicio
{
    private readonly BancoDbContext conexionDb;
    public TransaccionServicio(BancoDbContext conexionDb)
    {
        this.conexionDb = conexionDb;
    }
    public async Task DepositoAsync(DepositoRequest request)
    {
        var cuenta = await conexionDb.Cuenta.FindAsync(request.IdCuenta);
        if (cuenta == null) throw new InvalidOperationException("Cuenta no encontrada");

        cuenta.Credito(request.Monto);
        await conexionDb.SaveChangesAsync();
    }
}

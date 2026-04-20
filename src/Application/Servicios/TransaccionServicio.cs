using Application.DTOs;
using Application.Interfaces;
using Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Application.Servicios;

public class TransaccionServicio
{
    private readonly IBancoDbContext conexionDb;
    public TransaccionServicio(IBancoDbContext conexionDb)
    {
        this.conexionDb = conexionDb;
    }
    public async Task DepositoAsync(DepositoRequest request)
    {
        var cuenta = await conexionDb.Cuentas.FindAsync(request.IdCuenta);
        if (cuenta == null) throw new InvalidOperationException("Cuenta no encontrada");

        cuenta.Credito(request.Monto);
        await conexionDb.SaveChangesAsync();
    }
}

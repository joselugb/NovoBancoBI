using System.Data.Common;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Servicios;

public class TransaccionQueryServicios
{
    private readonly IBancoDbContext conexionDb;

    public TransaccionQueryServicios(IBancoDbContext conexionDb)
    {
        this.conexionDb = conexionDb;
    }

    public async Task<List<TransaccionResponse>> ObtenerHistorialAsync(Guid idCuenta, int pagina, int cantidadPorPagina)
    {
        return await conexionDb.Transacciones
            .Where(t => t.IdCuentaOrigen == idCuenta || t.IdCuentaDestino == idCuenta)
            .OrderByDescending(t => t.Fecha)
            .Skip((pagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .Select(t => new TransaccionResponse(
                t.Id,
                t.IdCuentaOrigen,
                t.IdCuentaDestino,
                t.Monto,
                t.TipoTransaccion,                
                t.EstadoTransaccion,                
                t.Fecha,
                t.Referencia
            ))
            .ToListAsync();
    }
}
    
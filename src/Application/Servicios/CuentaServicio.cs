using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Excepciones;
using Domain.Entidades;

namespace Application.Servicios;

public class CuentaServicio
{
    private readonly IBancoDbContext conexionDb;
    private readonly ClienteServicio clienteServicio;

    public CuentaServicio(IBancoDbContext conexionDb, ClienteServicio clienteServicio)
    {
        this.conexionDb = conexionDb;
        this.clienteServicio = clienteServicio;
    }

    /// <summary>
    /// Crea una nueva cuenta bancaria para un cliente existente.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CuentaResponse> CrearCuentaAsync(CuentaRequest request, CancellationToken cancellationToken = default)
    {
        var cliente = await conexionDb.Clientes.FirstOrDefaultAsync(c => c.DocumentoIdentidad == request.DocumentoIdentidad, cancellationToken);

        if (cliente is null)
        {
            throw new DominioExcepcion("Cliente no encontrado.");
        }

        var cuentaNueva = new Cuenta
        {
            Id = Guid.NewGuid(),
            IdCliente = cliente.Id,
            NumeroCuenta = GenerarNumeroCuenta(),
            Tipo = Domain.Enumeradores.TipoCuenta.AHORROS,
            Moneda = "USD",
            Balance = 0.00m,
            EstadosCuenta = Domain.Enumeradores.EstadosCuenta.ACTIVA,
            FechaCreacion = DateTime.UtcNow
        };

        await conexionDb.Cuentas.AddAsync(cuentaNueva, cancellationToken);
        await conexionDb.SaveChangesAsync(cancellationToken);

        return new CuentaResponse
        {
            Id = cuentaNueva.Id,
            ClienteId = cuentaNueva.IdCliente,
            NumeroCuenta = cuentaNueva.NumeroCuenta,
            Tipo = cuentaNueva.Tipo,
            Moneda = cuentaNueva.Moneda,
            Balance = cuentaNueva.Balance,
            EstadoCuenta = cuentaNueva.EstadosCuenta
        };
    }

    /// <summary>
    /// Obtiene los detalles de una cuenta bancaria específica por su ID.
    /// </summary>
    /// <param name="idCuenta"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CuentaResponse> ObtenerCuentaPorIdAsync(Guid idCuenta, CancellationToken cancellationToken = default)
    {
        var cuenta = await conexionDb.Cuentas.FirstOrDefaultAsync(c => c.Id == idCuenta, cancellationToken);

        if (cuenta is null)
        {
            throw new DominioExcepcion("Cuenta no encontrada.");
        }

        return new CuentaResponse
        {
            Id = cuenta.Id,
            ClienteId = cuenta.IdCliente,
            NumeroCuenta = cuenta.NumeroCuenta,
            Tipo = cuenta.Tipo,
            Moneda = cuenta.Moneda,
            Balance = cuenta.Balance,
            EstadoCuenta = cuenta.EstadosCuenta
        };
    }

    /// <summary>
    /// Bloquear cuenta
    /// </summary>
    /// <param name="idCuenta"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task BloquearCuentaAsync(Guid idCuenta, CancellationToken cancellationToken = default)
    {
        var cuenta = await conexionDb.Cuentas.FirstOrDefaultAsync(c => c.Id == idCuenta, cancellationToken);

        if (cuenta is null)
        {
            throw new DominioExcepcion("Cuenta no encontrada.");
        }

        if (cuenta.EstadosCuenta == Domain.Enumeradores.EstadosCuenta.BLOQUEADA)
        {
            throw new DominioExcepcion("Cuenta ya se encuentra bloqueada.");
        }

        cuenta.EstadosCuenta = Domain.Enumeradores.EstadosCuenta.BLOQUEADA;

        await conexionDb.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Cerrar cuenta
    /// </summary>
    /// <param name="idCuenta"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task CerrarCuentaAsync(Guid idCuenta, CancellationToken cancellationToken = default)
    {
        var cuenta = await conexionDb.Cuentas.FirstOrDefaultAsync(c => c.Id == idCuenta, cancellationToken);

        if (cuenta is null)
        {
            throw new DominioExcepcion("Cuenta no encontrada.");
        }

        if (cuenta.EstadosCuenta == Domain.Enumeradores.EstadosCuenta.CERRADA)
        {
            throw new DominioExcepcion("Cuenta ya se encuentra cerrada.");
        }

        cuenta.EstadosCuenta = Domain.Enumeradores.EstadosCuenta.CERRADA;

        await conexionDb.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Activar cuenta
    /// </summary>
    /// <param name="idCuenta"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ActivateAccountAsync(Guid idCuenta, CancellationToken cancellationToken = default)
    {
        var cuenta = await conexionDb.Cuentas.FirstOrDefaultAsync(c => c.Id == idCuenta, cancellationToken);

        if (cuenta is null)
        {
            throw new DominioExcepcion("Cuenta no encontrada.");
        }

        if (cuenta.EstadosCuenta == Domain.Enumeradores.EstadosCuenta.ACTIVA)
        {
            throw new DominioExcepcion("Cuenta ya se encuentra activa.");
        }

        cuenta.EstadosCuenta = Domain.Enumeradores.EstadosCuenta.ACTIVA;

        await conexionDb.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Genera un número de cuenta aleatorio.
    /// </summary>
    /// <returns>Número de cuenta generado.</returns>
    private static string GenerarNumeroCuenta()
    {
        var random = new Random();
        return $"{random.Next(10000000, 99999999)}";
    }
}
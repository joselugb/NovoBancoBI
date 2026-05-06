using Application.DTOs;
using Application.Interfaces;
using Domain.Entidades;
using Domain.Enumeradores;
using Domain.Excepciones;
using Microsoft.EntityFrameworkCore;

namespace Application.Servicios;

public class TransaccionServicio
{
    private readonly IBancoDbContext conexionDb;
    public TransaccionServicio(IBancoDbContext conexionDb)
    {
        this.conexionDb = conexionDb;
    }
    /// <summary>
    /// Deposita un monto específico en una cuenta bancaria.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DepositoAsync(DepositoRequest request, CancellationToken cancellationToken = default)
    {
        // Validación de monto positivo
        if (request.Monto <= 0)
        {
            throw new DominioExcepcion("Monto a depositar debe ser positivo.");
        }

        // Obtener la cuenta
        var cuenta = await conexionDb.Cuentas.FirstOrDefaultAsync(c => c.Id == request.IdCuenta, cancellationToken);

        if (cuenta is null)
        {
            throw new DominioExcepcion("Cuenta no encontrada.");
        }

        // Validar que la cuenta esté activa
        AsegurarCuentaActiva(cuenta);
        
        using var transaccion = await ((Microsoft.EntityFrameworkCore.DbContext)conexionDb).Database.BeginTransactionAsync();
        cuenta.Credito(request.Monto);

        this.conexionDb.Transacciones.Add(new Transaccion
        {
            Id = Guid.NewGuid(),
            IdCuentaOrigen = request.IdCuenta, //al ser un deposito la cuenta origen y destino es la misma
            IdCuentaDestino = request.IdCuenta,
            Monto = request.Monto,
            TipoTransaccion = (int)TiposTransacciones.DEPOSITO,
            EstadoTransaccion = (int)EstadoTransaccion.CORRECTA,
            Referencia = Guid.NewGuid().ToString(),
            Fecha = DateTime.UtcNow
        });

        await conexionDb.SaveChangesAsync(cancellationToken);
        await transaccion.CommitAsync(cancellationToken);        
    }

    /// <summary>
    /// Metodo para retirar un monto específico de una cuenta bancaria. 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task RetiroAsync(RetiroRequest request, CancellationToken cancellationToken = default)
    {
        // Validación de monto positivo
        if (request.Monto <= 0)
        {
            throw new DominioExcepcion("Monto a retirar debe ser positivo.");
        }

        var cuenta = await conexionDb.Cuentas.FirstOrDefaultAsync(c => c.Id == request.IdCuenta, cancellationToken);

        if (cuenta is null)
        {
            throw new DominioExcepcion("Cuenta no encontrada.");
        }

        // Validar que la cuenta esté activa
        AsegurarCuentaActiva(cuenta);

        // Validar referencia única
        var referenciaExiste = await conexionDb.Transacciones.FirstOrDefaultAsync(c => c.Referencia == request.Referencia, cancellationToken);

        if (referenciaExiste is not null)
        {
            throw new DominioExcepcion("Existe una transaccion con la misma referencia");
        }

        // Validar saldo suficiente
        if (cuenta.Balance < request.Monto)
        {
            throw new InsuficienteBalanceExcepcion();
        }

        await conexionDb.BeginTransactionAsync();

        // Aplicar lógica
        cuenta.Debito(request.Monto);        

        this.conexionDb.Transacciones.Add(new Transaccion
        {
            Id = Guid.NewGuid(),            
            IdCuentaOrigen = request.IdCuenta, //al ser un retiro la cuenta origen y destino es la misma
            IdCuentaDestino = request.IdCuenta,
            Monto = request.Monto,
            TipoTransaccion = TiposTransacciones.RETIRO,
            EstadoTransaccion = (int)EstadoTransaccion.CORRECTA,
            Referencia = Guid.NewGuid().ToString(),
            Fecha = DateTime.UtcNow
        });
                
        await conexionDb.SaveChangesAsync(cancellationToken);
        await conexionDb.CommitTransactionAsync(cancellationToken);  
    }

    public async Task TransferenciaAsync(TransferenciaRequest request, CancellationToken cancellationToken = default)
    {
         // Validaciones básicas
        if (request.Monto <= 0)
        {
            throw new DominioExcepcion("Monto a transferir debe ser positivo.");
        }

        if (request.IdCuentaOrigen == request.IdCuentaDestino)
        {
            throw new DominioExcepcion("La cuenta de origen y destino no pueden ser la misma deben ser diferentes.");
        }

        // Validar referencia única
        var referenciaExiste = await conexionDb.Transacciones.FirstOrDefaultAsync(c => c.Referencia == request.Referencia, cancellationToken);

        if (referenciaExiste is not null)
        {
            throw new DominioExcepcion("Existe una transaccion con la misma referencia");
        }

        await conexionDb.BeginTransactionAsync();
        try
        {
            var cuentaOrigen = await conexionDb.Cuentas.FirstOrDefaultAsync(c => c.Id == request.IdCuentaOrigen, cancellationToken) ?? throw new DominioExcepcion("Cuenta origen no encontrada");
            var cuentaDestino = await conexionDb.Cuentas.FirstOrDefaultAsync(c => c.Id == request.IdCuentaDestino, cancellationToken) ?? throw new DominioExcepcion("Cuenta destino no encontrada");

            if (cuentaOrigen is null)
                throw new DominioExcepcion("Cuenta origen no encontrada.");

            if (cuentaDestino is null)
                throw new DominioExcepcion("Cuenta destino no encontrada.");

            // Validar que las cuentas estén activas
            AsegurarCuentaActiva(cuentaOrigen);
            AsegurarCuentaActiva(cuentaDestino);

            // Validar saldo suficiente
            if (cuentaOrigen.Balance < request.Monto)
            {
                throw new InsuficienteBalanceExcepcion();
            }

            // Aplicar movimientos
            cuentaOrigen.Debito(request.Monto);
            cuentaDestino.Credito(request.Monto);

            string baseReference = Guid.NewGuid().ToString();

            // Registro de débito
            var transaccionDebito = new Transaccion
            {
                Id = Guid.NewGuid(),
                IdCuentaOrigen = request.IdCuentaOrigen,
                IdCuentaDestino = request.IdCuentaDestino,
                Monto = request.Monto,
                TipoTransaccion = TiposTransacciones.TRANSFERENCIA,
                EstadoTransaccion = (int)EstadoTransaccion.CORRECTA,
                Referencia = $"{baseReference}-OUT",
                Fecha = DateTime.UtcNow
            };

            // Registro de crédito
            var transaccionCredito = new Transaccion
            {
                Id = Guid.NewGuid(),
                IdCuentaOrigen = request.IdCuentaDestino,
                IdCuentaDestino = request.IdCuentaOrigen,
                Monto = request.Monto,
                TipoTransaccion = TiposTransacciones.TRANSFERENCIA,
                EstadoTransaccion = (int)EstadoTransaccion.CORRECTA,
                Referencia = $"{baseReference}-IN",
                Fecha = DateTime.UtcNow
            };
            
            conexionDb.Transacciones.Add(transaccionDebito);
            conexionDb.Transacciones.Add(transaccionCredito);
            await conexionDb.SaveChangesAsync(cancellationToken);
            await conexionDb.CommitTransactionAsync(cancellationToken);
        }
        catch(Exception)
        {
            await conexionDb.RollbackTransactionAsync(cancellationToken);
            throw;
        }        
    }

    /// <summary>
    /// Obtiene una lista paginada de transacciones asociadas a una cuenta bancaria específica.
    /// </summary>
    /// <param name="idCuenta"></param>
    /// <param name="pagina"></param>
    /// <param name="cantidadPorPagina"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task <(IReadOnlyList<TransaccionResponse> Items, int TotalCount)> ObtenerTransaccionesDeCuentaAsync(
        Guid idCuenta,
        int pagina,
        int cantidadPorPagina,
        CancellationToken cancellationToken = default)
    {
        var cuenta = await conexionDb.Cuentas.FirstOrDefaultAsync(c => c.Id == idCuenta, cancellationToken);

        if (cuenta is null)
        {
            throw new DominioExcepcion("Cuenta no encontrada.");
        }

        var transacciones = await ObtenerPaginasPorIdCuentaAsync(idCuenta, pagina, cantidadPorPagina, cancellationToken);

        var items = transacciones.Items.Select(x => new TransaccionResponse
        {
            Id = x.Id,
            IdCuentaOrigen = x.IdCuentaOrigen,
            IdCuentaDestino = (Guid)x.IdCuentaDestino,
            Monto = x.Monto,
            TipoTransaccion = x.TipoTransaccion,
            Referencia = x.Referencia,
            EstadoTransaccion = x.EstadoTransaccion,
            Fecha = x.Fecha
        }).ToList();

        return (items, transacciones.TotalCount);
    }

    public async Task <(IReadOnlyList<Transaccion> Items, int TotalCount)> ObtenerPaginasPorIdCuentaAsync(Guid idCuenta, int pagina, int cantidadPorPagina, CancellationToken cancellationToken = default)
    {
        var consulta = conexionDb.Transacciones
            .Where(x => x.IdCuentaOrigen == idCuenta || x.IdCuentaDestino == idCuenta)
            .OrderByDescending(x => x.Fecha);

        var totalCount = await consulta.CountAsync(cancellationToken);

        var items = await consulta
            .Skip((pagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
    /// <summary>
    /// Valida que la cuenta esté en estado activo.
    /// </summary>
    private static void AsegurarCuentaActiva(Cuenta cuenta)
    {
        if (cuenta.EstadosCuenta != EstadosCuenta.ACTIVA)
        {
            throw new DominioExcepcion("La cuenta no está activa.");
        }
    }
}

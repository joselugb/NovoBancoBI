
using Application.DTOs;
using Application.Servicios;
using Domain.Entidades;
using Domain.Enumeradores;
using Domain.Excepciones;
using Xunit;


public class TestsTransaccionServicio
{

    [Fact]
    public async Task DepositoPermitirIdempotencia()
    {
        using var db = TestDbContextFactory.Create();

        var cuenta = new Cuenta
        {
            Id = Guid.NewGuid(),
            EstadosCuenta = EstadosCuenta.ACTIVA
        };

        db.Cuentas.Add(cuenta);
        await db.SaveChangesAsync();

        var service = new TransaccionServicio(db);
        var request = new DepositoRequest(cuenta.Id, 100, "REF-001");

        await service.DepositoAsync(request);
        await service.DepositoAsync(request);

        Assert.Equal(100, cuenta.Balance);

    }


    [Fact]
    public async Task TransferenciaPermitidaRollbackCuandoDestinoBloqueado()
    {
        using var db = TestDbContextFactory.Create();

        var cuentaOrigen = new Cuenta
        {
            Id = Guid.NewGuid(),
            EstadosCuenta = EstadosCuenta.ACTIVA
        };

        var cuentaDestino = new Cuenta
        {
            Id = Guid.NewGuid(),
            EstadosCuenta = EstadosCuenta.BLOQUEADA
        };

        db.Cuentas.AddRange(cuentaOrigen, cuentaDestino);
        await db.SaveChangesAsync();

        var service = new TransaccionServicio(db);

        await Assert.ThrowsAsync<CuentaInactivaExcepcion>(() =>
            service.TransferenciaAsync(new TransferenciaRequest(cuentaOrigen.Id, cuentaDestino.Id, 50, "TRF-1"))
        );

        Assert.Equal(0, cuentaOrigen.Balance);
        Assert.Equal(0, cuentaDestino.Balance);
    }
}
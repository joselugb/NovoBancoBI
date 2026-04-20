using Xunit;
using Domain.Enumeradores;
using Domain.Entidades;
using Domain.Excepciones;

public class TestsCuentas
{
	[Fact]
	public void DebitoCuandoBalanceEsInsuficiente()
	{
		var cuenta = new Cuenta
		{
			Estado = EstadosCuenta.ACTIVA
		};

        Assert.Throws<InsuficienteBalanceExcepcion>(() => cuenta.Debito(100));
    }


    [Fact]
    public void DebitoCuandoCuentaEsBloqueada()
    {
        var cuenta = new Cuenta
        {
            Estado = EstadosCuenta.BLOQUEADA
        };

        Assert.Throws<CuentaInactivaExcepcion>(() => cuenta.Debito(100));
    }
}

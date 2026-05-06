using Domain.Enumeradores;
using Domain.Excepciones;

namespace Domain.Entidades;

public class Cuenta
{
    public Guid Id {get; set;}

    public Guid IdCliente {get; set;}
    public string NumeroCuenta {get; set;} = string.Empty;
    public decimal Balance {get; set;}
    public EstadosCuenta EstadosCuenta { get; set;} = 0;
    public TipoCuenta Tipo { get; set; }
    public string Moneda { get; set; } = string.Empty;
    public DateTime? FechaCreacion { get; set; } = DateTime.UtcNow;

    public Cliente? Cliente { get; set; }
    public void Credito(decimal monto)
    {
        Balance += monto;
    }
    public void Debito(decimal monto)
    {
        if(EstadosCuenta != EstadosCuenta.ACTIVA)
            throw new CuentaInactivaExcepcion();
                
        if(Balance < monto)
            throw new InsuficienteBalanceExcepcion();

        Balance -= monto;
    }
}
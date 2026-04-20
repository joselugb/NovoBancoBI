using Domain.Enumeradores;
using Domain.Excepciones;

namespace Domain.Entidades;

public class Cuenta
{
    public Guid Id {get; set;}
    public string NumeroCuenta {get; set;} = string.Empty;
    public decimal Balance {get; private set;}
    public EstadosCuenta Estado {get; set;}

    public void Credito(decimal monto)
    {
        Balance += monto;
    }
    public void Debito(decimal monto)
    {
        if(Estado != EstadosCuenta.ACTIVA)
            throw new CuentaInactivaExcepcion();
                
        if(Balance < monto)
            throw new InsuficienteBalanceExcepcion();

        Balance -= monto;
    }
}
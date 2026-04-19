using Domain.Enumeradores;

namespace Domain.Entidades;

public class Cuenta
{
    public Guid Id {get; set;}
    public decimal Balance {get; private set;}
    public int EstadosCuenta { get; set;} = 0;

    public string NumeroCuenta { get; set;} = string.Empty;

    public void Credito(decimal monto)
    {
        Balance += monto;
    }
    public void Debito(decimal monto)
    {
        if(Balance < monto)
            throw new InvalidOperationException("Balance insuficiente");
        Balance -= monto;
    }
}
using Domain.Enumeradores;

namespace Domain.Entidades;

public class EstadosCuenta
{
    public Guid Id {get; set;}
    public decimal Balance {get; private set;}
    public EstadosCuenta Estado {get; set;}

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
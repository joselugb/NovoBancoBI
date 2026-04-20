namespace Domain.Excepciones;

public class InsuficienteBalanceExcepcion : DominioExcepcion
{
    public InsuficienteBalanceExcepcion() 
        : base("Balance insuficiente"){}
}
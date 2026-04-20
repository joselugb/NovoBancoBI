namespace Application.DTOs;

public class RetiroRequest
{
    public Guid IdCuenta { get; }
    public decimal Monto { get; }
    public string Referencia { get; }

    public RetiroRequest(Guid idCuenta, decimal monto, string referencia)
    {
        IdCuenta = idCuenta;
        Monto = monto;
        Referencia = referencia;
    }
}
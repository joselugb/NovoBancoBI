using Microsoft.Identity.Client;

namespace Application.DTOs;

public class DepositoRequest
{
    public Guid IdCuenta { get; }
    public decimal Monto { get; }
    public string Referencia { get; }

    public DepositoRequest(Guid idCuenta, decimal monto, string referencia)
    {
        IdCuenta = idCuenta;
        Monto = monto;
        Referencia = referencia;
    }
}
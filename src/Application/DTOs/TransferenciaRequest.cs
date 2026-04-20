namespace Application.DTOs;

public class TransferenciaRequest
{
    public Guid IdCuentaOrigen { get; }
    public Guid IdCuentaDestino { get; }
    public decimal Monto { get; }
    public string Referencia { get; }

    public TransferenciaRequest(Guid idCuentaOrigen, Guid idCuentaDestino, decimal monto, string referencia)
    {
        IdCuentaOrigen = idCuentaOrigen;
        IdCuentaDestino = idCuentaDestino;
        Monto = monto;
        Referencia = referencia;
    }
}
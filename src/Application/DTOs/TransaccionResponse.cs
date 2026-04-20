using Domain.Enumeradores;

namespace Application.DTOs;

public class TransaccionResponse
{
    public Guid Id { get; set; }
    public Guid IdCuentaOrigen { get; set;}
    public Guid IdCuentaDestino { get; set;}
    public decimal Monto { get; set;}
    public TiposTransacciones TipoTransaccion { get; set;}
    public EstadoTransaccion EstadoTransaccion { get; set;}
    public DateTime Fecha { get; set;}
    public string Referencia { get; set;} = string.Empty;  
}


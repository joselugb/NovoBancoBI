using Domain.Enumeradores;

namespace Application.DTOs;

public class TransaccionResponse
{
    public Guid Id { get; set; }
    public Guid IdCuenta { get; set;}
    public Guid IdCuentaDestino { get; set;}
    public decimal Monto { get; set;}
    public TiposTransacciones TipoTransaccion { get; set;}
    public EstadoTransaccion EstadoTransaccion { get; set;}
    public DateTime Fecha { get; set;}
    public string Referencia { get; set;} = string.Empty;
    public TransaccionResponse( Guid id, Guid idCuenta, Guid idCuentaDestino, decimal monto, TiposTransacciones tipoTransaccion, EstadoTransaccion estadoTransaccion, DateTime fecha, string referencia)
    {
        Id = id;
        IdCuenta = idCuenta;
        IdCuentaDestino = idCuentaDestino;
        Monto = monto;
        TipoTransaccion = tipoTransaccion;
        EstadoTransaccion = estadoTransaccion;
        Fecha = fecha;
        Referencia = referencia;
    
    }
}


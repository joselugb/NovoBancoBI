
using Domain.Enumeradores;

namespace Domain.Entidades
{
    public class Transaccion
    {
        public Guid Id { get; set; }
        public Guid IdCuentaOrigen { get; set; }
        public Guid IdCuentaDestino { get; set; }
        public decimal Monto { get; set; }
        public string Referencia { get; set; } = string.Empty;
        public TiposTransacciones TipoTransaccion { get; set; }
        public EstadoTransaccion EstadoTransaccion { get; set; }
        public DateTime Fecha { get; set; }
    }
}

using Domain.Enumeradores;

namespace Application.DTOs
{
    public class CrearCuentaRequest
    {
        public Guid IdCliente { get; set; }

        /// <summary>
        /// Campo tipo de cuenta Ahorros(1) y Corriente(2)
        /// </summary>
        public TipoCuenta TipoCuenta { get; set; }
    }
}

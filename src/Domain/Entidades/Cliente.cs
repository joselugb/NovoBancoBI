
using Domain.Enumeradores;

namespace Domain.Entidades
{
    /// <summary>
    /// Representa los datos necesarios para crear un cliente.
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Identificador del cliente.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Nombre completo del cliente.
        /// </summary>
        public string NombreCompleto { get; set; } = string.Empty;
        /// <summary>
        /// Documento de identidad del cliente.
        /// </summary>
        public string DocumentoIdentidad { get; set; } = string.Empty;
    }
}

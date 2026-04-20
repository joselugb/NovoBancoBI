namespace Application.DTOs;

/// <summary>
/// Representa datos del cliente retornados por la aplicacion.
/// </summary>
public class ClienteResponse
{
    /// <summary>
    /// Identificador del cliente.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="nombreCompleto">Nombre completo del cliente.</param>
    /// <param name="documentoIdentidad">Documento de identidad del cliente.</param>
    public ClienteResponse(Guid id, string nombreCompleto, string documentoIdentidad)
    {
        Id = id;
        NombreCompleto = nombreCompleto;
        DocumentoIdentidad = documentoIdentidad;
    }
    /// <summary>
    /// Identificador del cliente.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre completo del cliente.
    /// </summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Numero de documento identidaddel cliente.
    /// </summary>
    public string DocumentoIdentidad { get; set; } = string.Empty;
}
namespace Application.DTOs;

/// <summary>
/// Representa los datos necesarios para crear un cliente.
/// </summary>
public class ClienteRequest{
    /// <summary>
    /// Numero de documento identidad del cliente.
    /// </summary>
    public string DocumentoIdentidad {get; set;} = string.Empty;

    /// <summary>
    /// Nombre completo del cliente.
    /// </summary>
    public string NombreCompleto {get; set;} = string.Empty;
}
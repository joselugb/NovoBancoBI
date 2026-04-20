using Domain.Enumeradores;

namespace Application.DTOs;

/// <summary> 
/// Representa los datos necesarios para crear una cuenta.
/// </summary>
public class CuentaRequest
{
    /// <summary>
    /// Identificador del cliente propietario de la cuenta.
    /// </summary>
    public string DocumentoIdentidad { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de cuenta (Ahorros, Corriente, etc.).
    /// </summary>
    public TipoCuenta TipoCuenta { get; set; }

    /// <summary>
    /// Moneda de la cuenta (USD, EUR, etc.).
    /// </summary>
    public string Moneda { get; set; } = string.Empty;
}
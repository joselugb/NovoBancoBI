using Domain.Enumeradores;

namespace Application.DTOs;
/// <summary>
/// Representa los datos de una cuenta retornados por la aplicacion.
/// </summary>
public class CuentaResponse
{
    /// <summary>
    /// Identificador de la cuenta.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Numero de cuenta.
    /// </summary>
    public string NumeroCuenta { get; set; } = string.Empty;

    /// <summary>
    /// Saldo actual de la cuenta.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Identificador del cliente propietario de la cuenta.
    /// </summary>
    public Guid ClienteId { get; set; }

    /// <summary>/ Tipo de cuenta (Ahorros, Corriente, etc.).
    /// </summary>
    public TipoCuenta Tipo { get; set; }

    /// <summary> Moneda de la cuenta (USD, EUR, etc.).
    /// </summary>
    public string Moneda { get; set; } = string.Empty;

    /// <summary> Estado de la cuenta (Activa, Inactiva, Cerrada).
    /// </summary>
    public EstadosCuenta EstadoCuenta { get; set; }
}
namespace Application.DTOs;

public record RetiroRequest(
    Guid IdCuenta,
    decimal Monto,
    string Referencia
);
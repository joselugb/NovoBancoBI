namespace Application.DTOs;

public record DepositoRequest(
    Guid IdCuenta,
    decimal Monto,
    string Referencia
);
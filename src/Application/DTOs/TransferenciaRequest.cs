namespace Application.DTOs;

public record TransferenciaRequest(
    Guid DesdeIdCuenta,
    Guid HastaIdCuenta,
    decimal Monto,
    string Referencia
);
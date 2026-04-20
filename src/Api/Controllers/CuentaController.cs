using Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/cuentas")]
public class CuentaController : ControllerBase
{
    private readonly TransaccionQueryServicios servicio;

    public CuentaController(TransaccionQueryServicios servicio)
    {
        this.servicio = servicio;
    }
    
    [HttpGet("{idCuenta}/transacciones")]
    public async Task<IActionResult> ObtenerHistorial(
        Guid idCuenta, 
        [FromQuery] int pagina = 1, 
        [FromQuery]int cantidadPorPagina = 20)
    {
        var result = await this.servicio.ObtenerHistorialAsync(idCuenta, pagina, cantidadPorPagina);
        return Ok(result);
    }
}

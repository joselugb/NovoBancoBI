using Application.DTOs;
using Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/cuentas")]
public class CuentaController : ControllerBase
{
    private readonly TransaccionServicio servicio;
    private readonly CuentaServicio cuentaServicio;

    public CuentaController(TransaccionServicio servicio, CuentaServicio cuentaServicio)
    {
        this.servicio = servicio;
        this.cuentaServicio = cuentaServicio;
    }
    
    [HttpGet("{idCuenta}/transacciones")]
    public async Task<IActionResult> ObtenerHistorialTransacciones(
        Guid idCuenta, 
        [FromQuery] int pagina = 1, 
        [FromQuery]int cantidadPorPagina = 20)
    {
        var result = await this.servicio.ObtenerTransaccionesDeCuentaAsync(idCuenta, pagina, cantidadPorPagina);
        return Ok(new
        {
            Pagina = pagina,
            CantidadPorPagina = cantidadPorPagina,
            result.TotalCount,
            result.Items
        });
    }

    /// </summary>
    /// <param name="request">request</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CrearCuenta([FromBody] CrearCuentaRequest request, CancellationToken cancellationToken)
    {
        var result = await this.cuentaServicio.CrearCuentaAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerCuentaPorId), new { id = result.Id }, result);
    }

    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerCuentaPorId(Guid id, CancellationToken cancellationToken)
    {
        var result = await this.cuentaServicio.ObtenerCuentaPorIdAsync(id, cancellationToken);
        return Ok(result);
    }
}

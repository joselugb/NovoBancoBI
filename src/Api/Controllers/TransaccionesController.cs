using Application.DTOs;
using Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/transacciones")]
public class TransaccionesController : ControllerBase
{
    private readonly TransaccionServicio servicio;

    public TransaccionesController(TransaccionServicio servicio)
    {
        this.servicio = servicio;
    }

    /// <summary>
    /// Endpoint para realizar un depósito en una cuenta bancaria.
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="cancellationToken">cancellationToken</param>
    [HttpPost("deposito")]
    public async Task<IActionResult> Deposito(
        [FromBody] DepositoRequest request,
        CancellationToken cancellationToken)
    {
        await this.servicio.DepositoAsync(request, cancellationToken);
        return Ok(new {message = "Deposito procesado satisfactoriamente."});
    }

    /// <summary>
    /// Endpoint para realizar una transferencia entre dos cuentas bancarias.
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns></returns>
    [HttpPost("transferencia")]
    public async Task<IActionResult> Transferencia(
        [FromBody] TransferenciaRequest request,
        CancellationToken cancellationToken)
    {
        await this.servicio.TransferenciaAsync(request, cancellationToken);
        return Ok(new { message = "Transferencia procesada satisfactoriamente" });
    }


    /// <summary>
    /// Endpoint para realizar un retiro de una cuenta bancaria.
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns></returns>
    [HttpPost("retiro")]
    public async Task<IActionResult> Retiro(
        [FromBody] RetiroRequest request,
        CancellationToken cancellationToken)
    {
        await this.servicio.RetiroAsync(request, cancellationToken);
        return Ok(new { message = "Retiro procesado satisfactoriamente." });
    }
}
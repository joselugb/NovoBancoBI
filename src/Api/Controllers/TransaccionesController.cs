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
    
    [HttpPost("deposito")]
    public async Task<IActionResult> Deposito(DepositoRequest request)
    {
        await this.servicio.DepositoAsync(request);
        return Ok();
    }

    [HttpPost("transferencia")]
    public async Task<IActionResult> Transferencia(TransferenciaRequest request)
    {
        await this.servicio.TransferenciaAsync(request);
        return Ok();
    }

}
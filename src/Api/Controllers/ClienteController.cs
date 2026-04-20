using Application.DTOs;
using Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
[Route("api/clientes")]
public class ClienteController : ControllerBase
{
    private readonly ClienteServicio _clienteServicio;

    public ClienteController(ClienteServicio clienteServicio)
    {
        _clienteServicio = clienteServicio;
    }

    /// <summary>
    /// Obtiene un cliente por su documento de identidad.
    /// </summary>
    [HttpPost("documento-identidad")]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerCuentaPorNumeroDocumento(
        [FromBody] ClienteRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _clienteServicio.ObtenerClienteDocumentoIdentidadAsync(request.DocumentoIdentidad);
        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    [HttpPost("crear")]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CrearCliente(
        [FromBody] ClienteRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _clienteServicio.CrearClienteAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerClienteId), new { id = result.Id }, result);
    }

    /// <summary>
    /// Obtiene un cliente por su ID.
    /// </summary>
    [HttpGet("{guid}")]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerClienteId(Guid guid, CancellationToken cancellationToken)
    {
        var result = await _clienteServicio.ObtenerClienteIdAsync(guid, cancellationToken);
        return Ok(result);
    }

}
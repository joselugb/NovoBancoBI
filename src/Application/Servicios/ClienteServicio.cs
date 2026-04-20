using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Excepciones;
using Domain.Entidades;

namespace Application.Servicios;

public class ClienteServicio
{
    private readonly IBancoDbContext conexionDb;

    public ClienteServicio(IBancoDbContext conexionDb)
    {
        this.conexionDb = conexionDb;
    }

    public async Task<ClienteResponse> ObtenerClienteDocumentoIdentidadAsync(string documentoIdentidad)
    {
        var cliente = await conexionDb.Clientes.FirstOrDefaultAsync(c => c.DocumentoIdentidad == documentoIdentidad);

        if (cliente is null)
        {
            throw new DominioExcepcion("Cliente no encontrado.");
        }

        return new ClienteResponse(cliente.Id, cliente.NombreCompleto, cliente.DocumentoIdentidad);
    }

    public async Task<ClienteResponse> CrearClienteAsync(ClienteRequest request, CancellationToken cancellationToken)
    {
        // Validacion de nombre
        if (string.IsNullOrWhiteSpace(request.NombreCompleto))
        {
            throw new DominioExcepcion("El nombre completo del cliente es obligatorio.");
        }

        // Validacion de documento de identidad
        if (string.IsNullOrWhiteSpace(request.DocumentoIdentidad))
        {
            throw new DominioExcepcion("El documento de identidad del cliente es obligatorio.");
        }

        // Validar duplicado por documento
        var clienteExistente = await conexionDb.Clientes.FirstOrDefaultAsync(c => c.DocumentoIdentidad == request.DocumentoIdentidad, cancellationToken);

        if (clienteExistente is not null)
        {
            throw new DominioExcepcion("El cliente ya está registrado.");
        }

        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            NombreCompleto = request.NombreCompleto.Trim().ToUpper(),
            DocumentoIdentidad = request.DocumentoIdentidad.Trim()
        };

        await conexionDb.Clientes.AddAsync(cliente, cancellationToken);
        await conexionDb.SaveChangesAsync(cancellationToken);

        return new ClienteResponse(cliente.Id, cliente.NombreCompleto, cliente.DocumentoIdentidad);
    }

    public async Task<ClienteResponse> ObtenerClienteIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cliente = await conexionDb.Clientes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (cliente is null)
        {
            throw new DominioExcepcion("Cliente no encontrado.");
        }

        return new ClienteResponse(cliente.Id, cliente.NombreCompleto, cliente.DocumentoIdentidad);
    }
}
    
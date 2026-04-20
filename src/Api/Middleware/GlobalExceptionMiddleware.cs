using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using Domain.Excepciones;

namespace Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate siguiente;
    private readonly ILogger<GlobalExceptionMiddleware> logger;

    public GlobalExceptionMiddleware(RequestDelegate siguiente, ILogger<GlobalExceptionMiddleware> logger)
    {
        this.siguiente = siguiente;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.siguiente(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var response = new { mensaje = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            this.logger.LogError(ex, "Error de dominioExcepcion: {Mensaje}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = ex switch
        {
            DominioExcepcion => HttpStatusCode.Conflict,
            ArgumentException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        var response = new 
        { 
            error = ex.GetType().Name,
            mensaje = ex.Message,
            traceId = context.TraceIdentifier
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
                
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
using FIAP.CloudGames.Domain.Exceptions;
using FIAP.CloudGames.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace FIAP.CloudGames.Api.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                AuthenticationException => StatusCodes.Status401Unauthorized,
                DomainException => StatusCodes.Status409Conflict,
                ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var message = ex switch
            {
                AuthenticationException => "Falha de autenticação.",
                DomainException => "Erro de regra de negócio.",
                ValidationException => "Erro de validação.",
                _ => "Erro interno no servidor."
            };

            var response = ApiResponse<string>.Fail(message, [ex.Message]);
            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
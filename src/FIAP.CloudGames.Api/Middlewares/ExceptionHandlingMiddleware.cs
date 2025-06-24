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
                ValidationException => StatusCodes.Status400BadRequest,
                AuthenticationException => StatusCodes.Status401Unauthorized,
                DomainException => StatusCodes.Status400BadRequest,
                ConflictException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var message = ex switch
            {
                ValidationException => "Erro de validação.",
                AuthenticationException => "Falha de autenticação.",
                DomainException => "Erro de regra de negócio.",
                ConflictException => "O dado informado já existe no sistema.",
                _ => "Erro interno no servidor."
            };

            var response = ApiResponse<string>.Fail(message, [ex.Message]);
            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
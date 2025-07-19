using FIAP.CloudGames.Domain.Exceptions;
using FIAP.CloudGames.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
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
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                AggregateValidationException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status400BadRequest,
                AuthenticationException => StatusCodes.Status401Unauthorized,
                DomainException => StatusCodes.Status400BadRequest,
                ConflictException => StatusCodes.Status409Conflict,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var message = ex switch
            {
                AggregateValidationException => "One or more validation errors occurred.",
                ValidationException => "Validation error.",
                AuthenticationException => "Authentication failure.",
                DomainException => "Business rule error.",
                ConflictException => "The provided data already exists in the system.",
                NotFoundException => "The requested resource was not found.",
                _ => "Internal server error."
            };

            var response = ApiResponse<string>.Fail(message, GetAggregateValidations(ex));
            var json = JsonSerializer.Serialize(response);
            logger.LogError(ex, ((HttpStatusCode)context.Response.StatusCode).ToString(), context.Request.Path);
            await context.Response.WriteAsync(json);
        }
    }

    public static List<string> GetAggregateValidations(Exception ex)
    {
        return ex is AggregateValidationException aggEx ? aggEx.Errors.ToList() : [ex.Message];
    }
}
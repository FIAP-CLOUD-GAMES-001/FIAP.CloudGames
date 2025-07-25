using FIAP.CloudGames.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace FIAP.CloudGames.Api.Filters;

public class ValidationFilter<T>(ILogger<ValidationFilter<T>> logger) : IAsyncActionFilter where T : class
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator == null)
        {
            await next();
            return;
        }

        var request = context.ActionArguments.Values.OfType<T>().FirstOrDefault();
        if (request == null)
        {
            context.Result = new ObjectResult(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid request type.",
                Errors = [$"Expected request of type {typeof(T).Name}."]
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

            logger.LogError("ValidationFilter: Request type is null or not of expected type, expectedType: {expectedTypeName}", typeof(T).Name);
            return;
        }

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            context.Result = new ObjectResult(new ApiResponse<string>
            {
                Success = false,
                Message = "Validation failed.",
                Errors = errors
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

            logger.LogError("ValidationFilter: Validation failed for request, requestType: {requestTypeName}, errors: {validationErrors}", typeof(T).Name, string.Join(", ", errors));
            return;
        }

        await next();
    }
}
using FIAP.CloudGames.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace FIAP.CloudGames.Api.Filters;

public class ValidationFilter<T> : IAsyncActionFilter where T : class
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
            return;
        }

        await next();
    }
}
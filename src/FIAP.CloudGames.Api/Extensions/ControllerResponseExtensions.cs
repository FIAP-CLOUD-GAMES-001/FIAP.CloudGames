using FIAP.CloudGames.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FIAP.CloudGames.Api.Extensions;
public static class ControllerResponseExtensions
{
    public static IActionResult ApiOk<T>(this ControllerBase controller, T data, string? message = null)
    {
        return controller.Ok(ApiResponse<T>.Ok(data, message));
    }

    public static IActionResult ApiFail(this ControllerBase controller, string message, List<string>? errors = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var response = ApiResponse<string>.Fail(message, errors);
        return controller.StatusCode((int)statusCode, response);
    }
}
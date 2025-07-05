using FIAP.CloudGames.Api.Extensions;
using FIAP.CloudGames.Domain.Interfaces.Services;
using FIAP.CloudGames.Domain.Models;
using FIAP.CloudGames.Domain.Requests.User;
using FIAP.CloudGames.Domain.Responses.User;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FIAP.CloudGames.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public class UserController(IUserService service, IValidator<RegisterUserRequest> validator) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        ValidationResult validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return this.ApiFail("Validation failed.", errors);
        }

        var userCreated = await service.RegisterAsync(request);
        return this.ApiOk(userCreated, "User registered successfully.", HttpStatusCode.Created);
    }
}
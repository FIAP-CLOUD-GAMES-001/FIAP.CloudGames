using FIAP.CloudGames.Api.Extensions;
using FIAP.CloudGames.Domain.Enums;
using FIAP.CloudGames.Domain.Interfaces.Services;
using FIAP.CloudGames.Domain.Models;
using FIAP.CloudGames.Domain.Requests.User;
using FIAP.CloudGames.Domain.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace FIAP.CloudGames.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public class UserController(IUserService service) : ControllerBase
{
    // POST /api/User/register
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        if (request is null)
            return this.ApiFail("Requisição inválida.");

        var userCreated = await service.RegisterAsync(request);
        return this.ApiOk(userCreated, "User registered successfully.", HttpStatusCode.Created);
    }

    // GET /api/User
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<List<UserResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await service.GetAllUsersAsync();
        return this.ApiOk(users, "Users retrieved successfully.");
    }

    // POST /api/User/create-user-admin
    [HttpPost("create-user-admin")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUserFromAdmin([FromBody] RegisterUserRequest request)
    {
        if (request is null)
            return this.ApiFail("Requisição inválida.");

        var created = await service.RegisterAsync(request);
        return this.ApiOk(created, "Admin created successfully.", HttpStatusCode.Created);
    }

    // PUT /api/User/{id}/role
    [HttpPut("{id}/role")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRole(int id, [FromQuery] Role role)
    {
        var updated = await service.UpdateUserRoleAsync(id, role);
        return this.ApiOk(updated, $"User role updated to {role}.");
    }

    // GET /api/User/me
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return this.ApiFail("Invalid token.", null, HttpStatusCode.Unauthorized);

        var user = await service.GetByIdAsync(userId);
        if (user is null)
            return this.ApiFail("User not found.", null, HttpStatusCode.NotFound);

        return this.ApiOk(user, "Profile retrieved successfully.");
    }
}
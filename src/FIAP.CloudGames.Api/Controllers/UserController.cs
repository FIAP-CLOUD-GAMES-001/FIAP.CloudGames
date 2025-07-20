using FIAP.CloudGames.Api.Extensions;
using FIAP.CloudGames.Api.Filters;
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
    /// <summary>
    /// Registers a new user with the provided information.
    /// </summary>
    /// <remarks>This endpoint creates a new user account based on the data provided in the request body.
    /// Validation is performed on the input, and appropriate error messages are returned if the input is
    /// invalid.</remarks>
    /// <param name="request">The user registration details, including required fields such as username, password, and email.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the registration operation. If successful, returns a 201
    /// Created response with the registered user details. If validation fails, returns a 400 Bad Request response with
    /// error messages.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [TypeFilter(typeof(ValidationFilter<RegisterUserRequest>))]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var userCreated = await service.RegisterAsync(request);
        return this.ApiOk(userCreated, "User registered successfully.", HttpStatusCode.Created);
    }

    /// <summary>
    /// Retrieves a list of all users in the system.
    /// </summary>
    /// <remarks>This method is restricted to users with the "Admin" role and requires authentication.  It
    /// returns a successful response containing the list of users if the operation completes successfully.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing an <see cref="ApiResponse{T}"/> with a list of <see
    /// cref="UserResponse"/> objects  and a success message. Returns a 200 OK status code on success.</returns>
    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<List<UserResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await service.GetAllUsersAsync();
        return this.ApiOk(users, "Users retrieved successfully.");
    }

    /// <summary>
    /// Creates a new user account with administrative privileges.
    /// </summary>
    /// <remarks>This endpoint is restricted to users with the "Admin" role. It validates the provided user
    /// registration details and creates a new user account if the validation succeeds. If validation fails, an error
    /// response is returned with the validation errors.</remarks>
    /// <param name="request">The user registration details required to create the account. This includes information such as username,
    /// password, and other user-specific data.</param>
    /// <returns>A response containing the details of the newly created user account, along with a success message, if the
    /// operation is successful. Returns a validation error response if the input data is invalid.</returns>
    [HttpPost("create-user-admin")]
    [Authorize(Roles = "Admin")]
    [TypeFilter(typeof(ValidationFilter<RegisterUserAdminRequest>))]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUserFromAdmin([FromBody] RegisterUserAdminRequest request)
    {
        var created = await service.RegisterAdminAsync(request);
        return this.ApiOk(created, "Admin created successfully.", HttpStatusCode.Created);
    }

    /// <summary>
    /// Updates the role of a user with the specified ID.
    /// </summary>
    /// <remarks>This endpoint is restricted to users with the "Admin" role. The role is updated based on the
    /// provided <paramref name="role"/> parameter. A successful update returns a response containing the updated user
    /// information.</remarks>
    /// <param name="id">The unique identifier of the user whose role is to be updated.</param>
    /// <param name="role">The new role to assign to the user. Must be a valid <see cref="ERole"/> value.</param>
    /// <returns>An <see cref="IActionResult"/> containing an <see cref="ApiResponse{T}"/> with the updated user information if
    /// the operation is successful.</returns>
    [HttpPut("{id}/role")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRole(int id, [FromQuery] ERole role)
    {
        var updated = await service.UpdateUserRoleAsync(id, role);
        return this.ApiOk(updated, $"User role updated to {role}.");
    }

    /// <summary>
    /// Retrieves the profile information of the currently authenticated user.
    /// </summary>
    /// <remarks>This method requires the caller to be authenticated and authorized. It extracts the user's
    /// identifier from the authentication token and retrieves the corresponding profile information.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing the user's profile information wrapped in an <see
    /// cref="ApiResponse{T}"/> object if the operation is successful, or an error response if the authentication token
    /// is invalid.</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return this.ApiFail("Invalid token.", null, HttpStatusCode.Unauthorized);

        var user = await service.GetByIdAsync(userId);

        return this.ApiOk(user, "Profile retrieved successfully.");
    }
}
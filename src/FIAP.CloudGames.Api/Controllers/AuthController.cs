using FIAP.CloudGames.Api.Extensions;
using FIAP.CloudGames.Domain.Interfaces.Services;
using FIAP.CloudGames.Domain.Models;
using FIAP.CloudGames.Domain.Requests.Auth;
using FIAP.CloudGames.Domain.Responses.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.CloudGames.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await service.LoginAsync(request);
        return this.ApiOk(user, "Login realizado com sucesso.");
    }
}
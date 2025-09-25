using System.Diagnostics.Metrics;
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
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    private readonly Counter<int> _requestCounter;
    private readonly Histogram<double> _requestDuration;

    public AuthController(IAuthService service, IMeterFactory meterFactory)
    {
        _service = service;
        var meter = meterFactory.Create("MetricsApi.Items");

        _requestCounter = meter.CreateCounter<int>(
            "items_requests_total",
            description: "Total number of requests to items endpoints");

        _requestDuration = meter.CreateHistogram<double>(
            "items_request_duration_seconds",
            description: "Duration of requests to items endpoints in seconds");
    }

    /// <summary>
    /// Authenticates a user based on the provided login request and returns the result.
    /// </summary>
    /// <remarks>This method processes the login request by delegating authentication to the underlying
    /// service. If the login is successful, the method returns an HTTP 200 response with the authenticated user
    /// information.</remarks>
    /// <param name="request">The login request containing the user's credentials. Must not be null.</param>
    /// <returns>An <see cref="IActionResult"/> containing the authentication result and a success message if the login is
    /// successful.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        _requestCounter.Add(1, new("method", "GET"), new("endpoint", "/api/Auth/login"));

        var user = await _service.LoginAsync(request);

        stopwatch.Stop();
        _requestDuration.Record(stopwatch.Elapsed.TotalSeconds, 
            new("method", "GET"), new("endpoint", "/api/Auth/login"));

        return this.ApiOk(user, "Login successful.");
    }
}
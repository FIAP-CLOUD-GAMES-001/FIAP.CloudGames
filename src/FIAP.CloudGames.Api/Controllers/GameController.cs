using FIAP.CloudGames.Api.Extensions;
using FIAP.CloudGames.Api.Filters;
using FIAP.CloudGames.Domain.Interfaces.Services;
using FIAP.CloudGames.Domain.Models;
using FIAP.CloudGames.Domain.Requests.Game;
using FIAP.CloudGames.Domain.Responses.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FIAP.CloudGames.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public class GameController(IGameService gameService) : ControllerBase
{
    /// <summary>
    /// Creates a new game based on the provided request data.
    /// </summary>
    /// <remarks>This action requires the caller to be authenticated and have the "Admin" role. If the request
    /// is invalid, an appropriate error response will be returned.</remarks>
    /// <param name="request">The request containing the details of the game to be created. Cannot be null.</param>
    /// <returns>An <see cref="IActionResult"/> containing the created game details wrapped in an <see cref="ApiResponse{T}"/>
    /// object, along with a status code of <see cref="StatusCodes.Status201Created"/>.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [TypeFilter(typeof(ValidationFilter<CreateGameRequest>))]
    [ProducesResponseType(typeof(ApiResponse<GameResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateGameRequest request)
    {
        var game = await gameService.CreateAsync(request);
        return this.ApiOk(game, "Game created successfully.", HttpStatusCode.Created);
    }

    /// <summary>
    /// Retrieves a list of games.
    /// </summary>
    /// <remarks>This method returns a collection of games wrapped in an API response object.  The response
    /// includes a success message and the list of games.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing an <see cref="ApiResponse{T}"/> object with a list of <see
    /// cref="GameResponse"/> instances. The response has a status code of 200 (OK) if the operation is successful.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<GameResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var games = await gameService.ListAsync();
        return this.ApiOk(games, "Games retrieved successfully.");
    }
}
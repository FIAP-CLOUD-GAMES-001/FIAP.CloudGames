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
public class OwnedGameController(IOwnedGameService ownedGameService) : ControllerBase
{
    /// <summary>
    /// Adds a new game to the user's library.
    /// </summary>
    /// <remarks>This method requires the caller to be authenticated, and the user ID in the request must
    /// match the authenticated user's ID. If the user ID is invalid or unauthorized, the method returns an error
    /// response.</remarks>
    /// <param name="request">The request containing details of the game to be added, including the user ID and game information.</param>
    /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Returns a 201 Created response with the
    /// added game details if successful, or a 401 Unauthorized response if the user ID in the request does not match
    /// the authenticated user's ID.</returns>
    [HttpPost]
    [ServiceFilter(typeof(OwnedGameAccessFilter))]
    [TypeFilter(typeof(ValidationFilter<AddOwnedGameRequest>))]
    [ProducesResponseType(typeof(ApiResponse<OwnedGameResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Add([FromBody] AddOwnedGameRequest request)
    {
        var ownedGame = await ownedGameService.AddAsync(request);
        return this.ApiOk(ownedGame, "Game added to library successfully.", HttpStatusCode.Created);
    }

    /// <summary>
    /// Retrieves the list of games owned by the specified user.
    /// </summary>
    /// <remarks>This endpoint requires the authenticated user to either match the specified user ID or have
    /// the "Admin" role. If the user is unauthorized, the method returns a 401 Unauthorized response.</remarks>
    /// <param name="userId">The ID of the user whose owned games are to be retrieved.</param>
    /// <returns>An <see cref="IActionResult"/> containing an <see cref="ApiResponse{T}"/> with a list of <see
    /// cref="OwnedGameResponse"/> objects  representing the user's owned games, along with a success message. Returns a
    /// 401 Unauthorized response if access is denied.</returns>
    [HttpGet("user/{userId}")]
    [ServiceFilter(typeof(OwnedGameAccessFilter))]
    [ProducesResponseType(typeof(ApiResponse<List<OwnedGameResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var ownedGames = await ownedGameService.GetByUserIdAsync(userId);
        return this.ApiOk(ownedGames, "Library retrieved successfully.");
    }
}
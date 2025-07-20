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
[Authorize(Roles = "Admin")]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public class PromotionController(IPromotionService promotionService) : ControllerBase
{
    /// <summary>
    /// Creates a new promotion based on the provided request data.
    /// </summary>
    /// <remarks>This method uses the <c>promotionService</c> to create the promotion asynchronously.  The
    /// response includes a success message and the HTTP status code 201 to indicate that  the resource was successfully
    /// created.</remarks>
    /// <param name="request">The request containing the details of the promotion to be created.  This parameter must not be <see
    /// langword="null"/> and must include all required fields.</param>
    /// <returns>An <see cref="IActionResult"/> containing the created promotion details wrapped in an  <see
    /// cref="ApiResponse{T}"/> object, along with a status code of 201 (Created).</returns>
    [HttpPost]
    [TypeFilter(typeof(ValidationFilter<CreatePromotionRequest>))]
    [ProducesResponseType(typeof(ApiResponse<PromotionResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePromotionRequest request)
    {
        var promotion = await promotionService.CreateAsync(request);
        return this.ApiOk(promotion, "Promotion created successfully.", HttpStatusCode.Created);
    }

    /// <summary>
    /// Retrieves a list of promotions.
    /// </summary>
    /// <remarks>This method returns a collection of promotions wrapped in an API response object.  The
    /// response includes a success message and the list of promotions.</remarks>
    /// <returns>An <see cref="IActionResult"/> containing an <see cref="ApiResponse{T}"/> object with a list of  <see
    /// cref="PromotionResponse"/> instances and a success message.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<PromotionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var promotions = await promotionService.ListAsync();
        return this.ApiOk(promotions, "Promotions retrieved successfully.");
    }
}
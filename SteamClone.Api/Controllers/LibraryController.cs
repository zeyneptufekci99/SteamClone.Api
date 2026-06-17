using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SteamClone.Api.Data;
using System.Security.Claims;

namespace SteamClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LibraryController: ControllerBase
{
    private readonly LibraryService _libraryService;
    private readonly GameService _gameService;

    public LibraryController (LibraryService libraryService, GameService gameService)
    {
        _libraryService = libraryService;
        _gameService = gameService;
    }

    [HttpPost("purchase/{gameId}")]
    public async Task<IActionResult> Purchase(string gameId)
    {

        if (string.IsNullOrWhiteSpace(gameId))
            return BadRequest("Game id is required");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(userId == null) {
            return Unauthorized();
        }

        var game = await _gameService.GeyByIdAsync(gameId);
        if(game == null)
        {
            return NotFound("Game not found");
        }

        var alreadyOwned = await _libraryService.HasGameAsync(userId, gameId);

        if (alreadyOwned)
        {
            return BadRequest("You already own this game");

        }

        await _libraryService.AddGameToLibraryAsync(userId, gameId);

        return Ok("Game purchased successfully");
    }
}

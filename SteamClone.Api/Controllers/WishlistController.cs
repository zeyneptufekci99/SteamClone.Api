using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SteamClone.Api.Data;
using System.Security.Claims;

namespace SteamClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class WishlistController : ControllerBase
{
    private readonly WishlistService _wishlistService;
    private readonly GameService _gameService;
    private readonly LibraryService _libraryService;

    public WishlistController(WishlistService wishlistService, GameService gameService, LibraryService libraryService)
    {
        _wishlistService = wishlistService;
        _gameService = gameService;
        _libraryService = libraryService;
    }

    [HttpPost("{gameId}")]
    public async Task<IActionResult> AddToWishlist(string gameId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var game = await _gameService.GetByIdAsync(gameId);
        if (game == null)
        {
            return NotFound("Game not found.");
        }

        var ownsGame = await _libraryService.HasGameAsync(userId, gameId);

        if (ownsGame)
        {
            return BadRequest("You already own this game.");
        }

        var alreadyInWishlist = await _wishlistService.ExistAsync(userId, gameId);

        if (alreadyInWishlist)
        {
            return BadRequest("Game is already in your wishlist.");

        }

        await _wishlistService.AddAsync(userId, gameId);

        return Ok("Game added successfully");
    }

    [HttpGet]
    public async Task<IActionResult> GetMyWishlist()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var wishlistItems = await _wishlistService.GetUserWhishlistAsync(userId);
        var games = new List<object>();

        foreach (var item in wishlistItems)
        {
            var game = await _gameService.GetByIdAsync(item.GameId);
            if (game != null)
            {
                games.Add(new
                {
                    game.Id,
                    game.Name,
                    game.Description,
                    game.Price,
                    game.CoverImageUrl,
                    item.CreatedAt
                });
            }
        }
        return Ok(games);
    }

    [HttpDelete("{gameId}")]
    public async Task<IActionResult> RemoveFromWishlist(string gameId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var exists = await _wishlistService.ExistAsync(userId, gameId);
        if (!exists)
        {
            return NotFound("Game not found in wishlist.");
        }

        await _wishlistService.RemoveAsync(userId, gameId);
        return Ok("Game removed from wishlist.");
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SteamClone.Api.Data;
using SteamClone.Api.DTOs;
using SteamClone.Api.Models;
using System.Security.Claims;

namespace SteamClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class ReviewController: ControllerBase
{
    private readonly ReviewService _reviewService;
    private readonly GameService _gameService;
    private readonly LibraryService _libraryService;

    public ReviewController(ReviewService reviewService, GameService gameService, LibraryService libraryService)
    {
        _reviewService = reviewService;
        _gameService = gameService;
        _libraryService = libraryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();

        if (request.Rating < 1 || request.Rating > 5)
            return BadRequest("Rating must be between 1 and 5");

        var game = await _gameService.GetByIdAsync(request.GameId);

        if (game == null)
            return NotFound("Game not found");

        var ownsGame = await _libraryService.HasGameAsync(
            userId,
            request.GameId);

        if (!ownsGame)
            return BadRequest("You must own the game before reviewing");

        var alreadyReviewed =
            await _reviewService.HasUserReviewedGameAsync(
                userId,
                request.GameId);

        if (alreadyReviewed)
            return BadRequest("You already reviewed this game");

        var review = new Review
        {
            UserId = userId,
            GameId = request.GameId,
            Rating = request.Rating,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await _reviewService.CreateAsync(review);

        return Ok("Review created successfully");
    }

    [HttpGet("game/{gameId}")]
    public async Task<IActionResult> GetGameById(string gameId)
    {
        var reviews = await _reviewService.GetByGameIdAsync(gameId);

        return Ok(reviews);
    }

    [HttpGet("game/{gameId}/summary")]
    public async Task<IActionResult> GetSummary(string gameId)
    {
        var averageRating = await _reviewService.GetAverageRatingAsync(gameId);
        var reviewCount = await _reviewService.GetReviewCountAsync(gameId);

        return Ok(new GameReviewSummaryResponse
        {
            AverageRating = averageRating,
            ReviewCount = reviewCount
        });
    }
}

using Microsoft.AspNetCore.Mvc;
using SteamClone.Api.Data;
using SteamClone.Api.DTOs;
using SteamClone.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace SteamClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController: ControllerBase
{
    private readonly GameService _gameService;

    public GamesController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
    {

       if(page < 1)
        {
            return BadRequest("Page number must be greater than 0");
        }

       var totalCount = await _gameService.GetCountAsync();
       var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        if (page > totalPages && totalCount > 0)
            return BadRequest("Page exceeds total page count");

        var games = await _gameService.GetPagedAsync(page, pageSize);

        return Ok(new PaginationResponse<Game>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Data = games
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var game = await _gameService.GetByIdAsync(id);
        if (game == null)
            return NotFound("Game not found");
        return Ok(game);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateGameRequest request)
    {
        var game = new Game
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CoverImageUrl = request.CoverImageUrl
        };

        await _gameService.CreateAsync(game);
        return Ok(game);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var game = await _gameService.GetByIdAsync(id);
        if (game == null)
            return NotFound("Game not found");
        await _gameService.DeleteAsync(id);
        return Ok("Game deleted successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateGameRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name)){
            return BadRequest("Game name is required");
        }
        if(request.Price < 0)
        {
            return BadRequest("Price cannot be negative");
        }
        var updated = await _gameService.UpdateAsync(id, request);

        if (!updated)
        {
            return NotFound("Game not Found");
        }

        return Ok("Game updated successfully");
    }
}

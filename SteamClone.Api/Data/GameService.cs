using SteamClone.Api.DTOs;
using SteamClone.Api.Models;
using SteamClone.Api.Repositories;

namespace SteamClone.Api.Data;

public class GameService
{
    private readonly GameRepository _gameRepository;

    public GameService(GameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<List<Game>> GetAllAsync()
    {
        return await _gameRepository.GetAllAsync();
    }

    public async Task<Game?> GetByIdAsync(string id)
    {
        return await _gameRepository.GetByIdAsync(id);
    }

    public async Task CreateAsync(Game game)
    {
        await _gameRepository.CreateAsync(game);
    }

    public async Task DeleteAsync(string id)
    {
        await _gameRepository.DeleteAsync(id);
    }

    public async Task<bool> UpdateAsync(string id, UpdateGameRequest request)
    {
        var existingGame = await _gameRepository.GetByIdAsync(id);

        if (existingGame == null)
            return false;

        existingGame.Name = request.Name;
        existingGame.Description = request.Description;
        existingGame.Price = request.Price;
        existingGame.CoverImageUrl = request.CoverImage;

        await _gameRepository.ReplaceAsync(id, existingGame);

        return true;
    }

    public async Task<long> GetCountAsync()
    {
        return await _gameRepository.GetCountAsync();
    }

    public async Task<long> GetFilteredCountAsync(string? search)
    {
        return await _gameRepository.GetFilteredCountAsync(search);
    }

    public async Task<List<Game>> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDirection)
    {
        return await _gameRepository.GetPagedAsync(
            page,
            pageSize,
            search,
            sortBy,
            sortDirection);
    }
}
using SteamClone.Api.Models;
using SteamClone.Api.Repositories;

namespace SteamClone.Api.Data;

public class LibraryService
{
    private readonly LibraryRepository _libraryRepository;

    public LibraryService(LibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }

    public async Task<bool> HasGameAsync(string userId, string gameId)
    {
        return await _libraryRepository.HasGameAsync(userId, gameId);
    }

    public async Task AddGameToLibraryAsync(string userId, string gameId)
    {
        var userGame = new UserGame
        {
            UserId = userId,
            GameId = gameId,
            PurchasedDate = DateTime.UtcNow
        };

        await _libraryRepository.AddGameToLibraryAsync(userGame);
    }

    public async Task<List<UserGame>> GetUserLibraryAsync(string userId)
    {
        return await _libraryRepository.GetUserLibraryAsync(userId);
    }
}
using MongoDB.Driver;
using SteamClone.Api.Data;
using SteamClone.Api.Models;

namespace SteamClone.Api.Repositories;

public class LibraryRepository
{
    private readonly IMongoCollection<UserGame> _userGames;

    public LibraryRepository(MongoDbService db)
    {
        _userGames = db.Database.GetCollection<UserGame>("UserGames");
    }

    public async Task<bool> HasGameAsync(string userId, string gameId)
    {
        return await _userGames
            .Find(x => x.UserId == userId && x.GameId == gameId)
            .AnyAsync();
    }

    public async Task AddGameToLibraryAsync(UserGame userGame)
    {
        await _userGames.InsertOneAsync(userGame);
    }

    public async Task<List<UserGame>> GetUserLibraryAsync(string userId)
    {
        return await _userGames
            .Find(x => x.UserId == userId)
            .ToListAsync();
    }
}
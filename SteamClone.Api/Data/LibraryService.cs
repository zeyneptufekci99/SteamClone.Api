using MongoDB.Driver;
using SteamClone.Api.Models;
namespace SteamClone.Api.Data;

public class LibraryService
{
    private readonly IMongoCollection<UserGame> _userGames;

    public LibraryService(MongoDbService db)
    {
        _userGames = db.Database.GetCollection<UserGame>("UserGames");
    }

    public async Task<bool> HasGameAsync(string userId, string gameId)
    {
        return await _userGames.Find(x=> x.UserId == userId && x.GameId == gameId).AnyAsync();
    }

    public async Task AddGameToLibraryAsync(string userId, string gameId)
    {
        var userGame = new UserGame
        {
            UserId = userId,
            GameId = gameId,
            PurchasedDate = DateTime.UtcNow
        };

        await _userGames.InsertOneAsync(userGame);
    }

    public async Task<List<UserGame>> GetUserLibraryAsync(string userId)
    {
        return await _userGames.Find(x => x.UserId == userId).ToListAsync();
    }

}

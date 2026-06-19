using MongoDB.Bson;
using MongoDB.Driver;
using SteamClone.Api.DTOs;
using SteamClone.Api.Models;

namespace SteamClone.Api.Data;

public class GameService
{
    private readonly IMongoCollection<Game> _games;

    public GameService(MongoDbService db)
    {
        _games = db.Database.GetCollection<Game>("Games");
    }

    public async Task<List<Game>> GetAllAsync()
    {

        return await _games.Find(_ => true).ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return null;

        return await _games.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Game game)
    {
        await _games.InsertOneAsync(game);
    }

    public async Task DeleteAsync(string id)
    {
        await _games.DeleteOneAsync(x => x.Id == id);
    }

    public async Task<bool> UpdateAsync(string id, UpdateGameRequest request)
    {
        var existingGame = await GetByIdAsync(id);

        if (existingGame == null)
        {
            return false;
        }

        existingGame.Name = request.Name;
        existingGame.Description = request.Description;
        existingGame.Price = request.Price;
        existingGame.CoverImageUrl = request.CoverImage;
        await _games.ReplaceOneAsync(x => x.Id == id, existingGame);
        return true;
    }

    public async Task<long> GetCountAsync()
    {
        return await _games.CountDocumentsAsync(_ => true);
    }

    public async Task<List<Game>> GetPagedAsync(int page, int pageSize)
    {
        return await _games.Find(_=> true).Skip((page-1)*pageSize).Limit(pageSize).ToListAsync();
    }

}

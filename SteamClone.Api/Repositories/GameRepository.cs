using MongoDB.Bson;
using MongoDB.Driver;
using SteamClone.Api.Data;
using SteamClone.Api.Models;

namespace SteamClone.Api.Repositories;

public class GameRepository
{
    private readonly IMongoCollection<Game> _games;

    public GameRepository(MongoDbService db)
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

    public async Task ReplaceAsync(string id, Game game)
    {
        await _games.ReplaceOneAsync(x => x.Id == id, game);
    }

    public async Task<long> GetCountAsync()
    {
        return await _games.CountDocumentsAsync(_ => true);
    }

    public async Task<long> GetFilteredCountAsync(string? search)
    {
        var filter = BuildSearchFilter(search);

        return await _games.CountDocumentsAsync(filter);
    }

    public async Task<List<Game>> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        string? sortDirection)
    {
        var filter = BuildSearchFilter(search);

        var query = _games.Find(filter);

        var isDescending = sortDirection?.ToLower() == "desc";

        query = sortBy?.ToLower() switch
        {
            "name" => isDescending
                ? query.SortByDescending(x => x.Name)
                : query.SortBy(x => x.Name),

            "price" => isDescending
                ? query.SortByDescending(x => x.Price)
                : query.SortBy(x => x.Price),

            _ => query.SortBy(x => x.Name)
        };

        return await query
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    private static FilterDefinition<Game> BuildSearchFilter(string? search)
    {
        var filter = Builders<Game>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(search))
        {
            filter = Builders<Game>.Filter.Regex(
                x => x.Name,
                new BsonRegularExpression(search, "i"));
        }

        return filter;
    }
}
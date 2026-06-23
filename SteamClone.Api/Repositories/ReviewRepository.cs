using MongoDB.Driver;
using SteamClone.Api.Data;
using SteamClone.Api.Models;

namespace SteamClone.Api.Repositories;

public class ReviewRepository
{
    private readonly IMongoCollection<Review> _reviews;

    public ReviewRepository(MongoDbService db)
    {
        _reviews = db.Database.GetCollection<Review>("Reviews");
    }

    public async Task<bool> HasUserReviewedGameAsync(string userId, string gameId)
    {
        return await _reviews
            .Find(x => x.UserId == userId && x.GameId == gameId)
            .AnyAsync();
    }

    public async Task CreateAsync(Review review)
    {
        await _reviews.InsertOneAsync(review);
    }

    public async Task<List<Review>> GetByGameIdAsync(string gameId)
    {
        return await _reviews
            .Find(x => x.GameId == gameId)
            .ToListAsync();
    }

    public async Task<long> GetReviewCountAsync(string gameId)
    {
        return await _reviews
            .CountDocumentsAsync(x => x.GameId == gameId);
    }
}
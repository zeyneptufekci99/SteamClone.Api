using MongoDB.Driver;
using SteamClone.Api.Models;

namespace SteamClone.Api.Data;

public class ReviewService
{
    public readonly IMongoCollection<Review> _reviews;
    public ReviewService(MongoDbService db)
    {
        _reviews = db.Database.GetCollection<Review>("Reviews");
    }

    public async Task CreateAsync(Review review)
    {
        await _reviews.InsertOneAsync(review);
    }

    public async Task<List<Review>> GetByGameIdAsync(string gameId)
    {
        return await _reviews.Find(x => x.GameId == gameId).ToListAsync();
    }

    public async Task<bool> HasUserReviewedGameAsync(string userId, string gameId)
    {
        return await _reviews.Find(x => x.UserId == userId && x.GameId == gameId).AnyAsync();
    }

    public async Task<double> GetAverageRatingAsync(string gameId) {

        var reviews = await _reviews.Find(x => x.GameId == gameId).ToListAsync();

        if (reviews.Any())
        {
            return 0;
        }

        return reviews.Average(x => x.Rating);
    }

    public async Task<int> GetReviewCount(string gameId)
    {
        return (int)await _reviews.CountDocumentsAsync(x=> x.GameId == gameId);
    }
}

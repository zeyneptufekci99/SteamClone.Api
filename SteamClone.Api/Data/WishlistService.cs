using MongoDB.Driver;
using SteamClone.Api.Models;
namespace SteamClone.Api.Data;

public class WishlistService
{
    private readonly IMongoCollection<WishlistItem> _wishlistItems;

    public WishlistService(MongoDbService db)
    {
        _wishlistItems = db.Database.GetCollection<WishlistItem>("WishlistItems");
    }
    public async Task<bool> ExistAsync(string userId, string gameId) {
    
        return await _wishlistItems.Find(x=> x.UserId == userId && x.GameId == gameId).AnyAsync();
    }

    public async Task AddAsync(string userId, string gameId)
    {
        var item = new WishlistItem
        {
            UserId = userId,
            GameId = gameId,
            CreatedAt = DateTime.UtcNow
        };

        await _wishlistItems.InsertOneAsync(item);
    }

    public async Task<List<WishlistItem>> GetUserWhishlistAsync(string userId)
    {
        return await _wishlistItems.Find(x => x.UserId == userId).ToListAsync();
    }

   public async Task RemoveAsync(string userId, string gameId)
    {
        await _wishlistItems.DeleteOneAsync(x=> x.UserId == userId && x.GameId == gameId);
    }
}

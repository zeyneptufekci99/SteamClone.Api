using MongoDB.Driver;
using SteamClone.Api.Data;
using SteamClone.Api.Models;

namespace SteamClone.Api.Repositories;

public class WishlistRepository
{
    private readonly IMongoCollection<WishlistItem> _wishlistItems;

    public WishlistRepository(MongoDbService db)
    {
        _wishlistItems = db.Database.GetCollection<WishlistItem>("WishlistItems");
    }

    public async Task<bool> ExistsAsync(string userId, string gameId)
    {
        return await _wishlistItems
            .Find(x => x.UserId == userId && x.GameId == gameId)
            .AnyAsync();
    }

    public async Task AddAsync(WishlistItem item)
    {
        await _wishlistItems.InsertOneAsync(item);
    }

    public async Task<List<WishlistItem>> GetUserWishlistAsync(string userId)
    {
        return await _wishlistItems
            .Find(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task RemoveAsync(string userId, string gameId)
    {
        await _wishlistItems.DeleteOneAsync(
            x => x.UserId == userId && x.GameId == gameId);
    }
}
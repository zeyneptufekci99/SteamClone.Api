using SteamClone.Api.Models;
using SteamClone.Api.Repositories;

namespace SteamClone.Api.Data;

public class WishlistService
{
    private readonly WishlistRepository _wishlistRepository;

    public WishlistService(WishlistRepository wishlistRepository)
    {
        _wishlistRepository = wishlistRepository;
    }

    public async Task<bool> ExistsAsync(string userId, string gameId)
    {
        return await _wishlistRepository.ExistsAsync(userId, gameId);
    }

    public async Task AddAsync(string userId, string gameId)
    {
        var item = new WishlistItem
        {
            UserId = userId,
            GameId = gameId,
            CreatedAt = DateTime.UtcNow
        };

        await _wishlistRepository.AddAsync(item);
    }

    public async Task<List<WishlistItem>> GetUserWishlistAsync(string userId)
    {
        return await _wishlistRepository.GetUserWishlistAsync(userId);
    }

    public async Task RemoveAsync(string userId, string gameId)
    {
        await _wishlistRepository.RemoveAsync(userId, gameId);
    }
}
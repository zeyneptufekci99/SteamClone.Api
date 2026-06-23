using MongoDB.Driver;
using SteamClone.Api.Data;
using SteamClone.Api.Models;

namespace SteamClone.Api.Repositories;

public class UserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(MongoDbService db)
    {
        _users = db.Database.GetCollection<User>("Users");
    }

    public async Task CreateAsync(User user)
    {
        await _users.InsertOneAsync(user);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _users
            .Find(x => x.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _users
            .Find(x => x.RefreshToken == refreshToken)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(string id, User user)
    {
        await _users.ReplaceOneAsync(x => x.Id == id, user);
    }
}
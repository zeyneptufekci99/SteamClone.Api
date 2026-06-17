using MongoDB.Driver;
using SteamClone.Api.Models;

namespace SteamClone.Api.Data;

public class UserService
{

    private readonly IMongoCollection<User> _user;

    public UserService(MongoDbService db)
    {
        _user = db.Database.GetCollection<User>("Users");
    }

    public async Task CreateAsync(User user)
    {
        await _user.InsertOneAsync(user);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _user.Find(x=> x.Email == email).FirstOrDefaultAsync();
    }
}

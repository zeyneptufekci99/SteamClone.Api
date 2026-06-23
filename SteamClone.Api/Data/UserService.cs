using SteamClone.Api.Models;
using SteamClone.Api.Repositories;

namespace SteamClone.Api.Data;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateAsync(User user)
    {
        await _userRepository.CreateAsync(user);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _userRepository.GetByRefreshTokenAsync(refreshToken);
    }

    public async Task UpdateAsync(string id, User user)
    {
        await _userRepository.UpdateAsync(id, user);
    }
}
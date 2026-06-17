using Microsoft.AspNetCore.Mvc;
using SteamClone.Api.Data;
using SteamClone.Api.DTOs;
using SteamClone.Api.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SteamClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController: ControllerBase
{
    private readonly UserService _userService;
    private readonly JwtService _jwtService;

    public AuthController(UserService userService, JwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existingUser = await _userService.GetByEmailAsync(request.Email);
        if (existingUser != null) {
            return BadRequest("User Already Exists");
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password)
        };

        await _userService.CreateAsync(user);

      return Ok("User Registered Successfully");


    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userService.GetByEmailAsync(request.Email);
        if (user == null)
        {
            return Unauthorized("Invalid credentials");
        }
        var hashed = HashPassword(request.Password);

        if(user.PasswordHash != hashed)
        {
            return Unauthorized("Invalid credentials");
        }

        var token = _jwtService.GenerateToken(user);

        return Ok(new
        {
            token
        });
    }

    [Authorize]
    [HttpGet("profile")]
    public IActionResult Profie()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new
        {
            userId,
            username,
            email

        });
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }


 

}

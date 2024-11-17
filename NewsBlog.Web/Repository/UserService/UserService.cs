using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsBlog.DataAccess;
using NewsBlog.Models.Dtos;
using NewsBlog.Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace NewsBlog.Business.UserService;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public UserService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> RegisterAsync(string username, string password, string role)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Username == username);
        if (userExists)
            throw new InvalidOperationException("Username already exists.");

        var user = new User
        {
            UserId = Guid.NewGuid().ToString(),
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return "Registration successful.";
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password.");

        var token = GenerateJwtToken(user);
        return token;
    }

    public async Task<UserDto> GetProfileAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        return new UserDto { UserId = user.UserId, Username = user.Username, Role = user.Role };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserId),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}






//public class UserService : IUserService
//{
//    private readonly ApplicationDbContext _context;
//    private readonly IConfiguration _configuration;

//    public UserService(ApplicationDbContext context, IConfiguration configuration)
//    {
//        _context = context;
//        _configuration = configuration;
//    }

//    public async Task<string> RegisterAsync(string username, string password, string role)
//    {
//        var userExists = await _context.Users.AnyAsync(u => u.Username == username);
//        if (userExists)
//            throw new InvalidOperationException("Username already exists.");

//        var user = new User
//        {
//            UserId = Guid.NewGuid().ToString(),
//            Username = username,
//            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
//            Role = role
//        };

//        _context.Users.Add(user);
//        await _context.SaveChangesAsync();

//        return "Registration successful.";
//    }

//    public async Task<string> LoginAsync(string username, string password)
//    {
//        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
//        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
//            throw new UnauthorizedAccessException("Invalid username or password.");

//        var token = GenerateJwtToken(user);
//        return token;
//    }

//    public async Task<UserDto> GetProfileAsync(string userId)
//    {
//        var user = await _context.Users.FindAsync(userId);
//        if (user == null)
//            throw new KeyNotFoundException("User not found.");

//        return new UserDto { UserId = user.UserId, Username = user.Username, Role = user.Role };
//    }

//    private string GenerateJwtToken(User user)
//    {
//        var claims = new[]
//        {
//            new Claim(ClaimTypes.Name, user.UserId),
//            new Claim(ClaimTypes.Role, user.Role)
//        };

//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//        var token = new JwtSecurityToken(
//            _configuration["Jwt:Issuer"],
//            _configuration["Jwt:Audience"],
//            claims,
//            expires: DateTime.UtcNow.AddHours(2),
//            signingCredentials: creds
//        );

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }
//}


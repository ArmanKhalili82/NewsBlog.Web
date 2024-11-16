using NewsBlog.Models.Dtos;

namespace NewsBlog.Business.UserService;

public interface IUserService
{
    Task<string> RegisterAsync(string username, string password, string role);
    Task<string> LoginAsync(string username, string password);
    Task<UserDto> GetProfileAsync(string userId);
}


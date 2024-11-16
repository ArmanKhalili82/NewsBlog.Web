namespace NewsBlog.Models.Models;

public class User
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } // "Admin" or "User"
}

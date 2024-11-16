using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsBlog.Business.UserService;
using NewsBlog.Models.Dtos;

namespace NewsBlog.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        await _userService.RegisterAsync(dto.Username, dto.Password, dto.Role);
        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _userService.LoginAsync(dto.Username, dto.Password);
        return Ok(new { Token = token });
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = User.Identity.Name;
        var profile = await _userService.GetProfileAsync(userId);
        return Ok(profile);
    }
}


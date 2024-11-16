using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsBlog.Business.LikeService;
using NewsBlog.Models.Models;

namespace NewsBlog.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LikesController : ControllerBase
{
    private readonly ILikeService _likeService;

    public LikesController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    [HttpPost("{blogId}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> AddLike(int blogId)
    {
        var userId = User.Identity.Name;
        await _likeService.AddLikeAsync(blogId, userId);
        //await _likeService.AddLikeAsync(blogId);
        return Ok(new { Message = "Like added successfully" });
    }

    [HttpDelete("{likeId}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> RemoveLike(int likeId)
    {
        var userId = User.Identity.Name;
        await _likeService.RemoveLikeAsync(likeId, userId);
        //await _likeService.RemoveLikeAsync(likeId);
        return Ok(new { Message = "Like removed successfully" });
    }

    [HttpGet("{blogId}/count")]
    [AllowAnonymous]
    public async Task<IActionResult> GetLikeCount(int blogId)
    {
        var likeCount = await _likeService.GetLikeCountByBlogIdAsync(blogId);
        return Ok(new { BlogId = blogId, LikeCount = likeCount });
    }
}


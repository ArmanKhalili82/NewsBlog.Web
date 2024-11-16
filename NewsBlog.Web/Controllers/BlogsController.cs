using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsBlog.Business.BlogService;
using NewsBlog.Models.Dtos;

namespace NewsBlog.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogsController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogsController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    // Get all blogs
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBlogs()
    {
        var blogs = await _blogService.GetAllBlogsAsync();
        return Ok(blogs);
    }

    // Get a single blog by ID
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBlogById(int id)
    {
        try
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            return Ok(blog);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // Get trending blogs
    [HttpGet("trending")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTrendingBlogs()
    {
        var blogs = await _blogService.GetTrendingBlogsAsync();
        return Ok(blogs);
    }

    // Create a new blog (Admin only)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateBlog([FromBody] BlogCreateDto blogDto)
    {
        var blog = await _blogService.CreateBlogAsync(blogDto);
        return CreatedAtAction(nameof(GetBlogById), new { id = blog.BlogId }, blog);
    }

    // Update an existing blog (Admin only)
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBlog(int id, [FromBody] BlogUpdateDto blogDto)
    {
        try
        {
            await _blogService.UpdateBlogAsync(id, blogDto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // Delete a blog (Admin only)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBlog(int id)
    {
        try
        {
            await _blogService.DeleteBlogAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}


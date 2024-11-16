using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsBlog.Business.CommentService;
using NewsBlog.Models.Dtos;

namespace NewsBlog.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // Get all comments for a specific blog
        [HttpGet("blog/{blogId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsByBlogId(int blogId)
        {
            var comments = await _commentService.GetCommentsByBlogIdAsync(blogId);
            return Ok(comments);
        }

        // Add a comment to a blog
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> AddComment([FromBody] CommentCreateDto commentDto)
        {
            try
            {
                var userId = User.Identity.Name; // Assuming user is authenticated and token contains their ID
                var comment = await _commentService.AddCommentAsync(commentDto, userId);
                return CreatedAtAction(nameof(GetCommentsByBlogId), new { blogId = commentDto.BlogId }, comment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Update a comment (Admin only)
        [HttpPut("{commentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] CommentUpdateDto commentDto)
        {
            try
            {
                await _commentService.UpdateCommentAsync(commentId, commentDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Delete a comment (Admin only)
        [HttpDelete("{commentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                await _commentService.DeleteCommentAsync(commentId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

}

using NewsBlog.Models.Dtos;

namespace NewsBlog.Business.CommentService;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetCommentsByBlogIdAsync(int blogId);
    Task<CommentDto> AddCommentAsync(CommentCreateDto commentDto, string userId);
    Task UpdateCommentAsync(int commentId, CommentUpdateDto commentDto);
    Task DeleteCommentAsync(int id);
}


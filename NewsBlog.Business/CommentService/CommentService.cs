using Microsoft.EntityFrameworkCore;
using NewsBlog.DataAccess;
using NewsBlog.Models.Dtos;
using NewsBlog.Models.Models;

namespace NewsBlog.Business.CommentService;

public class CommentService : ICommentService
{
    private readonly ApplicationDbContext _context;

    public CommentService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get comments for a specific blog
    public async Task<IEnumerable<CommentDto>> GetCommentsByBlogIdAsync(int blogId)
    {
        var comments = await _context.Comments.Where(c => c.BlogId == blogId).ToListAsync();

        return comments.Select(c => new CommentDto
        {
            CommentId = c.CommentId,
            Author = c.Author,
            Content = c.Content,
            CreatedAt = c.CreatedAt
        }).ToList();
    }

    // Add a comment
    //public async Task<CommentDto> AddCommentAsync(CommentCreateDto commentDto)
    //{
    //    var comment = new Comment
    //    {
    //        BlogId = commentDto.BlogId,
    //        Author = commentDto.Author,
    //        Content = commentDto.Content
    //    };

    //    _context.Comments.Add(comment);
    //    await _context.SaveChangesAsync();

    //    return new CommentDto
    //    {
    //        CommentId = comment.CommentId,
    //        Author = comment.Author,
    //        Content = comment.Content,
    //        CreatedAt = comment.CreatedAt
    //    };
    //}

    public async Task<CommentDto> AddCommentAsync(CommentCreateDto commentDto, string userId)
    {
        var blogExists = await _context.Blogs.AnyAsync(b => b.BlogId == commentDto.BlogId);
        if (!blogExists)
            throw new KeyNotFoundException("Blog not found.");

        var comment = new Comment
        {
            BlogId = commentDto.BlogId,
            Author = userId,
            Content = commentDto.Content
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return new CommentDto
        {
            CommentId = comment.CommentId,
            //BlogId = comment.BlogId,
            Author = comment.Author,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt
        };
    }

    public async Task UpdateCommentAsync(int commentId, CommentUpdateDto commentDto)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null)
            throw new KeyNotFoundException("Comment not found");

        comment.Content = commentDto.Content;

        _context.Entry(comment).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    // Delete a comment
    public async Task DeleteCommentAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);

        if (comment == null) throw new KeyNotFoundException("Comment not found");

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}


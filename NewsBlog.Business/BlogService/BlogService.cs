using Microsoft.EntityFrameworkCore;
using NewsBlog.DataAccess;
using NewsBlog.Models.Dtos;
using NewsBlog.Models.Models;

namespace NewsBlog.Business.BlogService;

public class BlogService : IBlogService
{
    private readonly ApplicationDbContext _context;

    public BlogService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all blogs
    public async Task<IEnumerable<BlogDto>> GetAllBlogsAsync()
    {
        var blogs = await _context.Blogs.Include(b => b.Comments)
                                        .Include(b => b.Likes)
                                        .ToListAsync();

        return blogs.Select(b => new BlogDto
        {
            BlogId = b.BlogId,
            Title = b.Title,
            Content = b.Content,
            CreatedAt = b.CreatedAt,
            Views = b.Views,
            LikesCount = b.Likes.Count,
            CommentsCount = b.Comments.Count
        }).ToList();
    }

    // Get a single blog by ID
    public async Task<BlogDto> GetBlogByIdAsync(int id)
    {
        var blog = await _context.Blogs.Include(b => b.Comments)
                                       .Include(b => b.Likes)
                                       .FirstOrDefaultAsync(b => b.BlogId == id);

        if (blog == null) throw new KeyNotFoundException("Blog not found");

        return new BlogDto
        {
            BlogId = blog.BlogId,
            Title = blog.Title,
            Content = blog.Content,
            CreatedAt = blog.CreatedAt,
            Views = blog.Views,
            LikesCount = blog.Likes.Count,
            CommentsCount = blog.Comments.Count
        };
    }

    // Create a new blog
    public async Task<BlogDto> CreateBlogAsync(BlogCreateDto blogDto)
    {
        var blog = new Blog
        {
            Title = blogDto.Title,
            Content = blogDto.Content
        };

        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();

        return new BlogDto
        {
            BlogId = blog.BlogId,
            Title = blog.Title,
            Content = blog.Content,
            CreatedAt = blog.CreatedAt,
            Views = blog.Views,
            LikesCount = blog.Likes.Count,
            CommentsCount = blog.Comments.Count
        };
    }

    // Update a blog
    public async Task UpdateBlogAsync(int id, BlogUpdateDto blogDto)
    {
        var blog = await _context.Blogs.FindAsync(id);

        if (blog == null) throw new KeyNotFoundException("Blog not found");

        blog.Title = blogDto.Title;
        blog.Content = blogDto.Content;

        _context.Entry(blog).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    // Delete a blog
    public async Task DeleteBlogAsync(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);

        if (blog == null) throw new KeyNotFoundException("Blog not found");

        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
    }

    // Get trending blogs
    public async Task<IEnumerable<BlogDto>> GetTrendingBlogsAsync()
    {
        var blogs = await _context.Blogs.Include(b => b.Comments)
                                        .Include(b => b.Likes)
                                        .ToListAsync();

        return blogs.OrderByDescending(b => b.Views + b.Likes.Count + b.Comments.Count)
                    .Take(5)
                    .Select(b => new BlogDto
                    {
                        BlogId = b.BlogId,
                        Title = b.Title,
                        Content = b.Content,
                        CreatedAt = b.CreatedAt,
                        Views = b.Views,
                        LikesCount = b.Likes.Count,
                        CommentsCount = b.Comments.Count
                    })
                    .ToList();
    }
}


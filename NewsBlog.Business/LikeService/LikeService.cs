using Microsoft.EntityFrameworkCore;
using NewsBlog.DataAccess;
using NewsBlog.Models.Models;

namespace NewsBlog.Business.LikeService;

public class LikeService : ILikeService
{
    private readonly ApplicationDbContext _context;

    public LikeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddLikeAsync(int blogId, string userId)
    {
        var alreadyLiked = await _context.Likes.AnyAsync(l => l.BlogId == blogId && l.UserId == userId);
        if (alreadyLiked)
            throw new InvalidOperationException("User has already liked this blog.");

        var like = new Like { BlogId = blogId, UserId = userId };
        _context.Likes.Add(like);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveLikeAsync(int likeId, string userId)
    {
        var like = await _context.Likes.FirstOrDefaultAsync(l => l.LikeId == likeId && l.UserId == userId);
        if (like == null)
            throw new KeyNotFoundException("Like not found or not authorized.");

        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetLikeCountByBlogIdAsync(int blogId)
    {
        return await _context.Likes.CountAsync(l => l.BlogId == blogId);
    }
}






//public class LikeService : ILikeService
//{
//    private readonly ApplicationDbContext _context;

//    public LikeService(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task AddLikeAsync(Like like)
//    {
//        _context.Likes.Add(like);
//        await _context.SaveChangesAsync();
//    }

//    public async Task RemoveLikeAsync(int id)
//    {
//        var like = await _context.Likes.FindAsync(id);
//        if (like == null) throw new KeyNotFoundException("Like not found");

//        _context.Likes.Remove(like);
//        await _context.SaveChangesAsync();
//    }

//    public async Task<int> GetLikeCountByBlogIdAsync(int blogId)
//    {
//        return await _context.Likes.CountAsync(l => l.BlogId == blogId);
//    }
//}


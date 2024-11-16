using NewsBlog.Models.Models;

namespace NewsBlog.Business.LikeService;

public interface ILikeService
{
    Task AddLikeAsync(int blogId, string userId);
    Task RemoveLikeAsync(int likeId, string userId);
    Task<int> GetLikeCountByBlogIdAsync(int blogId);
}

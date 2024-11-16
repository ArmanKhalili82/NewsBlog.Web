using NewsBlog.Models.Dtos;

namespace NewsBlog.Business.BlogService;

public interface IBlogService
{
    Task<IEnumerable<BlogDto>> GetAllBlogsAsync();
    Task<BlogDto> GetBlogByIdAsync(int id);
    Task<BlogDto> CreateBlogAsync(BlogCreateDto blog);
    Task UpdateBlogAsync(int id, BlogUpdateDto blog);
    Task DeleteBlogAsync(int id);
    Task<IEnumerable<BlogDto>> GetTrendingBlogsAsync();
}

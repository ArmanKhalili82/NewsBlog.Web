namespace NewsBlog.Models.Dtos;

public class BlogDto
{
    public int BlogId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Views { get; set; }
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
}

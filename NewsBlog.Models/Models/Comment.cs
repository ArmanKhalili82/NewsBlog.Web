namespace NewsBlog.Models.Models;

public class Comment
{
    public int CommentId { get; set; }
    public int BlogId { get; set; }
    public Blog Blog { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

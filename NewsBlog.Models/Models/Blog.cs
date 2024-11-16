namespace NewsBlog.Models.Models;

public class Blog
{
    public int BlogId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int Views { get; set; } = 0;
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Like> Likes { get; set; }
}

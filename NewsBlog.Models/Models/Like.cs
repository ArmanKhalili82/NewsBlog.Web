namespace NewsBlog.Models.Models;

public class Like
{
    public int LikeId { get; set; }
    public int BlogId { get; set; }
    public Blog Blog { get; set; }
    public string UserId { get; set; } // To track who liked
}

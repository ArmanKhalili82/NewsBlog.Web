namespace NewsBlog.Models.Dtos;

public class CommentDto
{
    public int CommentId { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}

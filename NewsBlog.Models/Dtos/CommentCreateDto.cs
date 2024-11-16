namespace NewsBlog.Models.Dtos;

public class CommentCreateDto
{
    public int BlogId { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
}

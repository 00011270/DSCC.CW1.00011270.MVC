// Project made by 00011270
// For CC module level 6 WIUT
namespace BlogPlatformMVC.Models
{

    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
    }
}

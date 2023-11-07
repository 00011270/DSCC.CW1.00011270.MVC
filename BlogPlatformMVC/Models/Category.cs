// Project made by 00011270
// For CC module level 6 WIUT
namespace BlogPlatformMVC.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Post>? Posts { get; set; }
    }
}

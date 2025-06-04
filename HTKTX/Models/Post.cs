using Microsoft.AspNetCore.Identity;

namespace HTKTX.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public string? ImagePath { get; set; } // đường dẫn ảnh
        public ICollection<Comment> Comments { get; set; }   // ✅ Quan hệ 1-nhiều
        public ICollection<PostLike> Likes { get; set; }

    }
}

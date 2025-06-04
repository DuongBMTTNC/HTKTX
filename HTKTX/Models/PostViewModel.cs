namespace HTKTX.Models
{
    public class PostViewModel
    {
        public string content { get; set; }
        public string title { get; set; }
        public IFormFile? image { get; set; } 
    }
}

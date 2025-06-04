using System.ComponentModel.DataAnnotations;

namespace HTKTX.Models
{
    public class RepairRequestViewModel
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string RoomNumber { get; set; }

        public IFormFile ImageFile { get; set; } // Ảnh hiện trạng hư hỏng
    }
}

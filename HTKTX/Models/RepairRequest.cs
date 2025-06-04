using System.ComponentModel.DataAnnotations;

namespace HTKTX.Models
{
    public class RepairRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CCCD { get; set; }

        [Required]
        public string RoomNumber { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;

        public string? ImagePath { get; set; } // lưu tên file hình ảnh
    }
}

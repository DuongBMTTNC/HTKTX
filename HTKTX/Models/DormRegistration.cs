using System.ComponentModel.DataAnnotations;

namespace HTKTX.Models
{
    public class DormRegistration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CCCD { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }

        [Required]
        public string RoomNumber { get; set; }
        public Room Room { get; set; }

        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }

        public bool IsApproved { get; set; } = false;
        public string CCCDFrontImagePath { get; set; }  // Lưu đường dẫn ảnh mặt trước
        public string CCCDBackImagePath { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    
}

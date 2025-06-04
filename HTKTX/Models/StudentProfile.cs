using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HTKTX.Models
{
    public class StudentProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Không tự động tăng
        public string CCCD { get; set; }  // Khóa chính

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public string UserId { get; set; }   // Liên kết với tài khoản Identity
        public IdentityUser User { get; set; }

        // Các thông tin khác nếu cần
        public string Gender { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string RoomTypeId { get; set; }
        public string RoomNumber { get; set; }
        
    }
}

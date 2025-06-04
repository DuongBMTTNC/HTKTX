using System.ComponentModel.DataAnnotations;

namespace HTKTX.Models
{
    public class DormRegistrationViewModel
    {
        [Required(ErrorMessage = "CCCD is required.")]
        public string CCCD { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }

        public string Gender { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Room Type is required.")]
        public string RoomTypeId { get; set; }

        [Required(ErrorMessage = "Room Number is required.")]
        public string RoomNumber { get; set; }



        [Required(ErrorMessage = "Front CCCD image is required.")]
        [Display(Name = "CCCD Front Image")]
        public IFormFile CCCDFrontImage { get; set; } // For file upload

        [Required(ErrorMessage = "Back CCCD image is required.")]
        [Display(Name = "CCCD Back Image")]
        public IFormFile CCCDBackImage { get; set; } // For file upload

        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu thuê.")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày kết thúc (tùy chọn)")]
        public DateTime EndDate { get; set; }

        public DateTime DateOfBirth { get; set; } // Thêm trường ngày sinh nếu cần

    }
}

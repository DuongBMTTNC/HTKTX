using System.ComponentModel.DataAnnotations;

namespace HTKTX.Models
{
    public class Room
    {
        [Key]
        public string RoomNumber { get; set; }

        public int CurrentOccupants { get; set; }

        public string Gender { get; set; }

        // Khóa ngoại tới RoomType
        [Required]
        public string RoomTypeId { get; set; }

        public RoomType RoomType { get; set; }

        public ICollection<DormRegistration> DormRegistrations { get; set; }
    }
}

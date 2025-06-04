using System.ComponentModel.DataAnnotations;

namespace HTKTX.Models
{
    public class RoomType
    {
        [Key]
        [Required]
        public string RoomTypeId { get; set; }  // VD: "LT01", "VIP", "THUONG"

        [Required]
        public string Name { get; set; }        // VD: "Phòng 4 giường", "Phòng VIP"

        public int NumberOfBeds { get; set; }   // Số giường

        public string Facilities { get; set; }  // VD: "Máy lạnh, WC riêng, Wifi, Bàn học,..."

        public decimal Price { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}

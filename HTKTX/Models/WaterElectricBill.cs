using System.ComponentModel.DataAnnotations;

namespace HTKTX.Models
{
    public class WaterElectricBill
    {
        public int Id { get; set; }

        [Required]
        public string RoomNumber { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        // Điện
        public int ElectricityStart { get; set; }
        public int ElectricityEnd { get; set; }

        // Nước
        public int WaterStart { get; set; }
        public int WaterEnd { get; set; }

        [Required]
        public decimal ElectricityUnitPrice { get; set; }

        [Required]
        public decimal WaterUnitPrice { get; set; }

        public decimal ElectricityTotal => (ElectricityEnd - ElectricityStart) * ElectricityUnitPrice;
        public decimal WaterTotal => (WaterEnd - WaterStart) * WaterUnitPrice;
        public decimal Total => ElectricityTotal + WaterTotal;

        public Room Room { get; set; }
        public bool IsPaid { get; set; } = false;
    }
}

namespace HTKTX.Models
{
    public class UtilityBill
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public DateTime BillingMonth { get; set; }

        public int ElectricityUsage { get; set; }
        public int WaterUsage { get; set; }

        public decimal ElectricityPricePerUnit { get; set; } = 3000;
        public decimal WaterPricePerUnit { get; set; } = 10000;

        public decimal TotalAmount =>
            (ElectricityUsage * ElectricityPricePerUnit) +
            (WaterUsage * WaterPricePerUnit);

        public bool IsPaid { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

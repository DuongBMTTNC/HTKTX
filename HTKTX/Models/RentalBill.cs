using Microsoft.AspNetCore.Identity;

namespace HTKTX.Models
{
    public class RentalBill
    {
        public int Id { get; set; }
        public string StudentCCCD { get; set; }
        public string RoomNumber { get; set; }
        public string RoomTypeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPaid { get; set; } = false;
        public StudentProfile Student { get; set; }
        public RoomType RoomType { get; set; }
    }
}

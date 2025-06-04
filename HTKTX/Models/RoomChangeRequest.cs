namespace HTKTX.Models
{
    public class RoomChangeRequest
    {
        public int Id { get; set; }
        public string StudentCCCD { get; set; }
        public string CurrentRoomNumber { get; set; }
        public string DesiredRoomNumber { get; set; }
        public string Reason { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; } = false;
        public bool IsRejected { get; set; } = false;

        public StudentProfile Student { get; set; }
        public Room CurrentRoom { get; set; }
        public Room DesiredRoom { get; set; }
    }
}

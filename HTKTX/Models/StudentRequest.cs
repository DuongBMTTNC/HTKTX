using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HTKTX.Models
{
    public class StudentRequest
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public string CurrentRoomNumber { get; set; }
        public string DesiredRoomId { get; set; }

        public string Reason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;
        public bool IsRejected { get; set; } = false;

        public Room CurrentRoom { get; set; }
        public Room DesiredRoom { get; set; }
        public IdentityUser User { get; set; }
    }
}

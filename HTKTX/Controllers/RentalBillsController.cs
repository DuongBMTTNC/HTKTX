using HTKTX.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTKTX.Controllers
{
    public class RentalBillsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        
        private readonly ApplicationDbContext _context;
        public RentalBillsController(UserManager<IdentityUser> userManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.StudentProfiles.FirstOrDefaultAsync(s => s.Email == user.Email);

            if (student == null)
                return NotFound();

            var bills = await _context.RentalBills
                .Include(b => b.RoomType)
                .Where(b => b.StudentCCCD == student.CCCD)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bills);
        }
        public async Task<IActionResult> Details(int id)
        {
            var bill = await _context.RentalBills
                .Include(b => b.RoomType)
                .Include(b => b.Student)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
                return NotFound();

            var registration = await _context.DormRegistrations
                .Where(r => r.CCCD == bill.StudentCCCD && r.RoomNumber == bill.RoomNumber && r.IsApproved)
                .OrderByDescending(r => r.Id) // lấy lần đăng ký gần nhất
                .FirstOrDefaultAsync();

            ViewBag.StartDate = registration?.StartDate;
            ViewBag.EndDate = registration?.EndDate;

            return View(bill);
        }

    }
}

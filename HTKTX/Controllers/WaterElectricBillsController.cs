using HTKTX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTKTX.Controllers
{
    public class WaterElectricBillsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WaterElectricBillsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Index - Danh sách hóa đơn
        public async Task<IActionResult> Index()
        {
            var bills = await _context.WaterElectricBills
                .OrderByDescending(b => b.Year)
                .ThenByDescending(b => b.Month)
                .ToListAsync();

            return View(bills);
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Rooms = await _context.Rooms.ToListAsync();
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WaterElectricBill bill)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.WaterElectricBills.Add(bill);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentIndex()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null) return NotFound();

            var roomNumber = profile.RoomNumber;

            var bills = await _context.WaterElectricBills
                .Where(b => b.RoomNumber == roomNumber)
                .OrderByDescending(b => b.Month)
                .ToListAsync();

            return View(bills);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentDetails(int id)
        {
            var bill = await _context.WaterElectricBills
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
                return NotFound();

            return View(bill);
        }
    }
}

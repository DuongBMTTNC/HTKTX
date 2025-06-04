using HTKTX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTKTX.Controllers
{
    public class StudentProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _context.StudentProfiles.ToListAsync();
            return View(students);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Profile()
        {
            var userEmail = User.Identity.Name; // Giả sử đăng nhập bằng email

            var student = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.Email == userEmail);

            if (student == null)
            {
                return NotFound("Không tìm thấy thông tin sinh viên.");
            }

            return View(student);
        }
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DormInfo()
        {
            var userEmail = User.Identity.Name;

            // Lấy thông tin sinh viên từ Email
            var student = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.Email == userEmail);

            if (student == null)
            {
                return NotFound("Không tìm thấy thông tin sinh viên.");
            }

            // Lấy đăng ký nội trú mới nhất của sinh viên dựa vào CCCD
            var registration = await _context.DormRegistrations
                .Include(d => d.RoomType)
                .Include(d => d.Room)
                .Where(d => d.CCCD == student.CCCD && d.IsApproved)
                .OrderByDescending(d => d.RegisterDate)
                .FirstOrDefaultAsync();

            if (registration == null)
            {
                ViewBag.Message = "Bạn chưa có thông tin lưu trú nào được duyệt.";
                return View();
            }

            return View(registration);
        }
    }
}

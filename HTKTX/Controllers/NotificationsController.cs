using HTKTX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HTKTX.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public NotificationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ Danh sách thông báo dành cho user đang đăng nhập
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(notifications);
        }

        // ✅ Admin tạo thông báo mới
       
        // ✅ Xử lý form tạo thông báo
       

        // ✅ Đánh dấu là đã đọc
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null || notification.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ✅ Xoá thông báo (tuỳ chọn)
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null || notification.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CreateForStudents()
        {
            return View();
        }

        // Gửi thông báo đến tất cả sinh viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForStudents(CreateNotificationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Lấy danh sách user có vai trò là Student
                var students = await _userManager.GetUsersInRoleAsync("Student");

                foreach (var student in students)
                {
                    var notification = new Notification
                    {
                        UserId = student.Id,
                        Message = $"{model.Title}: {model.Message}",
                        CreatedAt = DateTime.Now,
                        IsRead = false
                    };
                    _context.Notifications.Add(notification);
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = "Đã gửi thông báo đến tất cả sinh viên.";
                return RedirectToAction("Index", "Notifications");
            }

            return View(model);
        }
        public async Task<IActionResult> AdminIndex()
        {
            var notifications = await _context.Notifications
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(notifications);
        }
    }
}

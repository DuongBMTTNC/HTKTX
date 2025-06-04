using HTKTX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTKTX.Controllers
{
    public class RepairRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public RepairRequestsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Sinh viên gửi yêu cầu
        [Authorize(Roles = "Student")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create(RepairRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var student = await _context.StudentProfiles.FirstOrDefaultAsync(s => s.Email == User.Identity.Name);
            if (student == null)
            {
                return NotFound("Không tìm thấy sinh viên.");
            }

            string ImagePath = await SaveImage(model.ImageFile);

            var request = new RepairRequest
            {
                Description = model.Description,
                RoomNumber = model.RoomNumber,
                CCCD = student.CCCD,
                RequestDate = DateTime.Now,
                ImagePath = ImagePath,
                IsApproved = false
            };

            _context.RepairRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Gửi yêu cầu sửa chữa thành công.";
            return RedirectToAction("MyRequests");
        }
        private async Task<string> SaveImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                // Log this for debugging, but don't throw, just return null/empty path
                Console.WriteLine("Lỗi: File ảnh rỗng hoặc không được chọn.");
                return null;
            }

            // Thêm kiểm tra loại file để đảm bảo chỉ chấp nhận ảnh
            if (!imageFile.ContentType.StartsWith("image/"))
            {
                Console.WriteLine($"Lỗi: Loại file không hợp lệ: {imageFile.ContentType}");
                return null;
            }

            // Giới hạn kích thước file (ví dụ: 5MB)
            const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
            if (imageFile.Length > MaxFileSize)
            {
                Console.WriteLine($"Lỗi: Kích thước file quá lớn: {imageFile.Length} bytes.");
                return null;
            }


            try
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/repair");

                // Tạo thư mục nếu nó không tồn tại
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Trả về đường dẫn tương đối để lưu vào database
                return "/uploads/repair/" + fileName;
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi chi tiết vào console hoặc hệ thống log
                Console.WriteLine($"Lỗi khi lưu ảnh: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                // In ra InnerException nếu có để biết thêm chi tiết
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return null; // Trả về null khi có lỗi
            }
        }
        // Admin xem danh sách yêu cầu
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var list = await _context.RepairRequests.ToListAsync();
            return View(list);
        }

        // Admin duyệt yêu cầu
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkAsApproved(int id)
        {
            var request = await _context.RepairRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.IsApproved = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyRequests()
        {
            var email = User.Identity.Name;

            var student = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.Email == email);

            if (student == null)
            {
                return NotFound("Không tìm thấy sinh viên.");
            }

            var requests = await _context.RepairRequests
                .Where(r => r.CCCD == student.CCCD)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            return View(requests);
        }
    }
}

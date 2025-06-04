using Microsoft.AspNetCore.Mvc;
using HTKTX.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering; // Thêm để dùng .ToList()

namespace HTKTX.Controllers
{
    public class DormRegistrationsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DormRegistrationsController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _userManager = userManager;
        }

        // GET: DormRegistration/Register
        public IActionResult Register()
        {
            // Đảm bảo rằng bạn truyền dữ liệu cho các dropdown
            // Kiểm tra null ở đây nếu _context có thể null (ví dụ: trong unit tests mà không mock DbContext)
            ViewBag.RoomTypes = _context.RoomTypes?.ToList() ?? new List<RoomType>();
            ViewBag.Rooms = _context.Rooms?.ToList() ?? new List<Room>();

            return View(new DormRegistrationViewModel()); // Luôn truyền một instance của ViewModel
        }

        // POST: DormRegistration/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(DormRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem RoomType và Room có tồn tại trong DB không
                var roomTypeExists = await _context.RoomTypes.AnyAsync(rt => rt.RoomTypeId == model.RoomTypeId);
                var roomExists = await _context.Rooms.AnyAsync(r => r.RoomNumber == model.RoomNumber);

                if (!roomTypeExists)
                {
                    ModelState.AddModelError("RoomTypeId", "Loại phòng không tồn tại.");
                }
                if (!roomExists)
                {
                    ModelState.AddModelError("RoomNumber", "Số phòng không tồn tại.");
                }

                if (!ModelState.IsValid)
                {
                    // Nếu có lỗi sau khi kiểm tra DB, re-populate dropdowns và trả về View
                    ViewBag.RoomTypes = _context.RoomTypes?.ToList() ?? new List<RoomType>();
                    ViewBag.Rooms = _context.Rooms?.ToList() ?? new List<Room>();
                    return View(model);
                }

                // Lưu ảnh CCCD
                string cccdFrontImagePath = await SaveImage(model.CCCDFrontImage);
                string cccdBackImagePath = await SaveImage(model.CCCDBackImage);

                // Kiểm tra xem việc lưu ảnh có thành công không
                if (string.IsNullOrEmpty(cccdFrontImagePath) || string.IsNullOrEmpty(cccdBackImagePath))
                {
                    ModelState.AddModelError("", "Không thể tải lên một hoặc cả hai ảnh CCCD. Vui lòng thử lại.");
                    // Re-populate dropdowns and return view
                    ViewBag.RoomTypes = _context.RoomTypes?.ToList() ?? new List<RoomType>();
                    ViewBag.Rooms = _context.Rooms?.ToList() ?? new List<Room>();
                    return View(model);
                }

                // Kiểm tra xem CCCD đã được đăng ký trước đó chưa
                var existingRegistration = await _context.DormRegistrations
                                                    .FirstOrDefaultAsync(dr => dr.CCCD == model.CCCD);
                if (existingRegistration != null)
                {
                    ModelState.AddModelError("CCCD", "Số CCCD này đã được đăng ký trước đó.");
                    ViewBag.RoomTypes = _context.RoomTypes?.ToList() ?? new List<RoomType>();
                    ViewBag.Rooms = _context.Rooms?.ToList() ?? new List<Room>();
                    return View(model);
                }


                // Tạo đối tượng DormRegistration từ ViewModel
                var registration = new DormRegistration
                {
                    CCCD = model.CCCD,
                    Email = model.Email,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    Phone = model.Phone,
                    RoomTypeId = model.RoomTypeId,
                    RoomNumber = model.RoomNumber,
                    RegisterDate = DateTime.Now,
                    IsApproved = false, // Mặc định là chưa duyệt
                    CCCDFrontImagePath = cccdFrontImagePath,
                    CCCDBackImagePath = cccdBackImagePath,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    DateOfBirth = model.DateOfBirth
                };

                _context.DormRegistrations.Add(registration);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Đăng ký thành công! Đang chờ phê duyệt.";
                return RedirectToAction("RegistrationConfirmation");
            }

            // Nếu ModelState không hợp lệ ban đầu (do validation attributes)
            ViewBag.RoomTypes = _context.RoomTypes?.ToList() ?? new List<RoomType>();
            ViewBag.Rooms = _context.Rooms?.ToList() ?? new List<Room>();
            return View(model);
        }

        // Phương thức lưu ảnh
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
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

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
                return "/uploads/" + fileName;
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

        // Action cho trang xác nhận đăng ký
        public IActionResult RegistrationConfirmation()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var registrations = await _context.DormRegistrations
                .Include(r => r.RoomType)
                .Include(r => r.Room)
                .ToListAsync();

            return View(registrations);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var registration = await _context.DormRegistrations
                .Include(r => r.RoomType)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (registration == null)
                return NotFound();

            return View(registration);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var registration = await _context.DormRegistrations.Include(r => r.Room)
.FirstOrDefaultAsync(r => r.Id == id);
            if (registration == null)
                return NotFound();

            if (registration.IsApproved)
            {
                TempData["Info"] = "Đơn này đã được duyệt.";
                return RedirectToAction("Index");
            }

            // Tạo tài khoản cho sinh viên
            var user = new IdentityUser
            {
                UserName = registration.Email,
                Email = registration.Email
            };

            var result = await _userManager.CreateAsync(user, "DefaultPassword123!"); // Gán mật khẩu mặc định

            if (!result.Succeeded)
            {
                TempData["Error"] = "Không thể tạo tài khoản: " + string.Join(", ", result.Errors.Select(e => e.Description));
                return RedirectToAction("Index");
            }

            // Gán role sinh viên
            await _userManager.AddToRoleAsync(user, "Student");

            // Tạo hồ sơ sinh viên (StudentProfile) từ thông tin đăng ký
            var profile = new StudentProfile
            {
                FullName = registration.FullName,
                CCCD = registration.CCCD,
                Email = registration.Email,
                UserId = user.Id,
                Gender = registration.Gender,
                Phone = registration.Phone,
                DateOfBirth = registration.DateOfBirth,
                RoomTypeId = registration.RoomTypeId,
                RoomNumber = registration.RoomNumber
                
            };

            _context.StudentProfiles.Add(profile);

            int totalMonths = ((registration.EndDate.Year - registration.StartDate.Year) * 12) +
                  registration.EndDate.Month - registration.StartDate.Month + 1; // +1 để tính luôn tháng đầu

            // Lấy giá phòng theo loại
            var roomPrice = await _context.RoomTypes
                .Where(rt => rt.RoomTypeId == registration.RoomTypeId)
                .Select(rt => rt.Price)
                .FirstOrDefaultAsync();

            // Tạo hóa đơn tiền phòng
            var bill = new RentalBill
            {
                StudentCCCD = registration.CCCD,
                RoomNumber = registration.RoomNumber,
                RoomTypeId = registration.RoomTypeId,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                CreatedAt = DateTime.Now,
                Amount = totalMonths * roomPrice
            };
            _context.RentalBills.Add(bill);
            // Đánh dấu đã duyệt
            registration.IsApproved = true;
            registration.Room.CurrentOccupants += 1;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã duyệt và cấp tài khoản cho sinh viên.";
            return RedirectToAction(nameof(Index));
        }

       

    }
}
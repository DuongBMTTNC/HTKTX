using HTKTX.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTKTX.Controllers
{
    public class PostsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PostsController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts.Include(p => p.User).OrderByDescending(p => p.CreatedAt).ToListAsync();
            return View(posts);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                string ImagePath = await SaveImage(model.image);

                var post = new Post
                {
                    Title = model.title,
                    Content = model.content,
                    CreatedAt = DateTime.Now,
                    UserId = _userManager.GetUserId(User),
                    ImagePath = ImagePath
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }


        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.Posts
        .Include(p => p.User) // người đăng bài
        .Include(p => p.Comments) // bình luận
            .ThenInclude(c => c.User) // người bình luận
        .Include(p => p.Likes) // like
        .FirstOrDefaultAsync(p => p.Id == id);

            var commentTest = await _context.Comments
    .Where(c => c.PostId == id)
    .Include(c => c.User)
    .ToListAsync();

            Console.WriteLine("Số bình luận: " + commentTest.Count);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts
    .Include(p => p.User)           // Tải user của bài đăng
    .Include(p => p.Likes)          // Nếu có Like để hiển thị
    .Include(p => p.Comments)       // Tải các bình luận
        .ThenInclude(c => c.User)   // Tải User của từng bình luận
    .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.Id) return NotFound();
            var existingPost = await _context.Posts.FindAsync(id);
            if (existingPost.UserId != _userManager.GetUserId(User)) return Unauthorized();

            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.UserId != _userManager.GetUserId(User)) return Unauthorized();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int PostId, string Content)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(Content))
            {
                return RedirectToAction("Details", new { id = PostId });
            }

            var comment = new Comment
            {
                PostId = PostId,
                Content = Content,
                CreatedAt = DateTime.Now,
                UserId = userId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = PostId });
        }

    // Toggle Like tương tự bạn có thể viết


        [HttpPost]
        public async Task<IActionResult> ToggleLike(int postId)
        {
            var userId = _userManager.GetUserId(User);
            var existing = await _context.PostLikes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            if (existing != null)
            {
                _context.PostLikes.Remove(existing);
            }
            else
            {
                _context.PostLikes.Add(new PostLike
                {
                    PostId = postId,
                    UserId = userId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = postId });
        }
       
    }

}

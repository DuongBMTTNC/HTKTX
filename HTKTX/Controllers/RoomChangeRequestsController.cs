using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using HTKTX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize]
public class RoomChangeRequestsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public RoomChangeRequestsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Sinh viên: Xem yêu cầu đổi phòng của chính mình
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (profile == null)
            return NotFound();

        var requests = await _context.RoomChangeRequests
            .Include(r => r.CurrentRoom)
            .Include(r => r.DesiredRoom)
            .Where(r => r.StudentCCCD == profile.CCCD)
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();

        return View(requests);
    }

    // Sinh viên: Gửi yêu cầu đổi phòng
    public async Task<IActionResult> Create()
    {
        var user = await _userManager.GetUserAsync(User);
        var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (profile == null) return NotFound();

        var currentRoom = profile.RoomNumber;
        var currentGender = profile.Gender;

        var rooms = await _context.Rooms
            .Where(r => r.RoomNumber != currentRoom && r.Gender == currentGender)
            .ToListAsync();

       
        ViewBag.CurrentRoomNumber = currentRoom;
        ViewBag.Rooms = new SelectList(rooms, "RoomNumber", "RoomNumber");


        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoomChangeRequest request)
    {
        var user = await _userManager.GetUserAsync(User);
        var profile = await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);

        if (profile == null) return NotFound();

        request.StudentCCCD = profile.CCCD;
        request.CurrentRoomNumber = profile.RoomNumber;
        request.RequestedAt = DateTime.Now;
        request.IsApproved = false;
        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            request.Reason = "Không có lý do cụ thể.";
        }
        _context.Add(request);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Yêu cầu đổi phòng đã được gửi.";
        return RedirectToAction(nameof(Index));
    }

    // Chi tiết yêu cầu
    public async Task<IActionResult> Details(int id)
    {
        var request = await _context.RoomChangeRequests
            .Include(r => r.CurrentRoom)
            .Include(r => r.DesiredRoom)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (request == null)
            return NotFound();

        return View(request);
    }

    // Admin xem toàn bộ yêu cầu
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminIndex()
    {
        var requests = await _context.RoomChangeRequests
            .Include(r => r.CurrentRoom)
            .Include(r => r.DesiredRoom)
            .Include(r => r.Student)
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();

        return View(requests);
    }

    // Admin duyệt yêu cầu
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Approve(int id)
    {
        var request = await _context.RoomChangeRequests
            .Include(r => r.CurrentRoom)
            .Include(r => r.DesiredRoom)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (request == null || request.IsApproved)
        {
            TempData["Error"] = "Yêu cầu không tồn tại hoặc đã được duyệt.";
            return RedirectToAction(nameof(AdminIndex));
        }

        var student = await _context.StudentProfiles
            .FirstOrDefaultAsync(s => s.CCCD == request.StudentCCCD);
        if (student == null) return NotFound();

        // Cập nhật số lượng người trong phòng
        var oldRoom = request.CurrentRoom;
        var newRoom = request.DesiredRoom;

        if (oldRoom != null) oldRoom.CurrentOccupants--;
        if (newRoom != null) newRoom.CurrentOccupants++;

        // Cập nhật phòng cho sinh viên
        student.RoomNumber = request.DesiredRoomNumber;

        request.IsApproved = true;
        await _context.SaveChangesAsync();

        TempData["Success"] = "Đã duyệt yêu cầu đổi phòng.";
        return RedirectToAction(nameof(AdminIndex));
    }
}

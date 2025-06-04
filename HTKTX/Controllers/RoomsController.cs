using HTKTX.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HTKTX.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms.Include(r => r.RoomType).ToListAsync();
            return View(rooms);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.RoomTypes = _context.RoomTypes
        .Select(rt => new SelectListItem
        {
            Value = rt.RoomTypeId,   // sẽ dùng để binding với RoomTypeId
            Text = rt.Name              // hiển thị tên loại phòng
        })
        .ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room)
        {
            room.CurrentOccupants = 0; // Khởi tạo số lượng người hiện tại là 0
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Rooms.Add(room);
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

        public async Task<IActionResult> Edit(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            ViewBag.RoomTypes = _context.RoomTypes
                .Select(rt => new SelectListItem
                {
                    Value = rt.RoomTypeId.ToString(),
                    Text = rt.Name
                }).ToList();

            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Room room)
        {
            if (id != room.RoomNumber) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Update(room);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                if (!_context.Rooms.Any(e => e.RoomNumber == room.RoomNumber))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Nếu có lỗi, load lại danh sách RoomTypes và trả về View
            ViewBag.RoomTypes = _context.RoomTypes
                .Select(rt => new SelectListItem
                {
                    Value = rt.RoomTypeId.ToString(),
                    Text = rt.Name
                }).ToList();

            return View(room);
        }


        public async Task<IActionResult> Delete(string id)
        {
            var room = await _context.Rooms.Include(r => r.RoomType).FirstOrDefaultAsync(m => m.RoomNumber == id);
            if (room == null) return NotFound();

            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            var room = await _context.Rooms.Include(r => r.RoomType).FirstOrDefaultAsync(m => m.RoomNumber == id);
            if (room == null) return NotFound();

            return View(room);
        }

        public async Task<IActionResult> SearchRoomByNumber(string roomNumber)
        {
            if (string.IsNullOrEmpty(roomNumber))
            {
                TempData["Error"] = "Vui lòng nhập số phòng cần tìm.";
                return RedirectToAction("Index");
            }

            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);

            if (room == null)
            {
                TempData["Error"] = $"Không tìm thấy phòng {roomNumber}.";
                return RedirectToAction("Index");
            }

            // Trả về danh sách chỉ chứa 1 phòng
            var rooms = new List<Room> { room };
            return View("Index", rooms); // Dùng lại View danh sách Index
        }

        [HttpGet]
        public IActionResult GetRooms(string roomTypeId, string gender)
        {
            var rooms = _context.Rooms
                .Include(r => r.RoomType) // Bao gồm thông tin loại phòng
                .Where(r => r.RoomTypeId == roomTypeId
                            && r.Gender == gender
                            && r.RoomType.NumberOfBeds > r.CurrentOccupants) // còn chỗ trống
                .Select(r => new {
                    r.RoomNumber
                    
                })
                .ToList();

            return Json(rooms);
        }

    }
}

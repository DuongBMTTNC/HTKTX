using HTKTX.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebKTX.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public UserManagementController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var userRoles = new List<UserWithRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserWithRolesViewModel
                {
                    User = user,
                    Roles = roles.ToList()
                });
            }

            return View(userRoles);
        }

        public async Task<IActionResult> AddUser()
        {
            // Tạo các role nếu chúng chưa tồn tại
            await EnsureRolesExist("admin", "staff", "student");
            ViewBag.Roles = _roleManager.Roles.ToList(); // Truyền danh sách roles cho View
            return View();
        }

        private async Task EnsureRolesExist(params string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        // POST: UserManagement/AddUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(CreateUserViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                
                if (result.Succeeded)
                {
                    // Thêm vào Role (nếu có)
                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                        if (!roleResult.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            ModelState.AddModelError(string.Empty, "Failed to add user to role.");
                            ViewBag.Roles = _roleManager.Roles.ToList();
                            return View(model);
                        }
                    }
                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    await transaction.RollbackAsync();
                    ViewBag.Roles = _roleManager.Roles.ToList();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                ViewBag.Roles = _roleManager.Roles.ToList();
                return View(model);
            }

            ViewBag.Roles = _roleManager.Roles.ToList();
            return View(model);
        }

       


    }
}

using Microsoft.AspNetCore.Mvc;

namespace HTKTX.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}

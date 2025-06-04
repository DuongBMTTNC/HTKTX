using Microsoft.AspNetCore.Mvc;

namespace HTKTX.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}

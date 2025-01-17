using Microsoft.AspNetCore.Mvc;

namespace LMS_WEB.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

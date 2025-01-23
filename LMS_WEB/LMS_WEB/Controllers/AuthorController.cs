using Microsoft.AspNetCore.Mvc;

namespace LMS_WEB.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

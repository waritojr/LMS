using LMS_WEB.Entities;
using LMS_WEB.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LMS_WEB.Helpers;

namespace LMS_WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserModel _userModel;

        public UserController(IUserModel userModel)
        {
            _userModel = userModel;
        }

        [HttpGet]
        [SecurityFilter]
        public IActionResult Profile()
        {
            var data = _userModel.GetUser(0);
            return View(data);
        }

        [HttpPost]
        [SecurityFilter]
        public IActionResult Profile(UserEnt entity)
        {
            var resp = _userModel.UpdateProfile(entity);

            if (resp == 1)
            {
                HttpContext.Session.SetString("user_name", entity.full_name);
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Message = "No se pudo actualizar su cuenta";
                return View();
            }
        }

        [HttpGet]
        [SecurityFilter]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [SecurityFilter]
        public IActionResult ChangePassword(UserEnt entity)
        {
            if (entity.password_prev.Trim() == entity.password_user.Trim())
            {
                ViewBag.Message = "Ingrese un contraseña nueva.";
            }

            var resp = _userModel.ChangePassword(entity);

            if (resp == 1)
                return RedirectToAction("Index", "Login");
            else
            {
                ViewBag.Message = "No se pudo actualizar su contraseña.";
                return View();
            }
        }
    }
}

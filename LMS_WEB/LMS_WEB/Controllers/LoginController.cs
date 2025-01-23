using LMS_WEB.Entities;
using LMS_WEB.Interfaces;
using LMS_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LMS_WEB.Controllers
{
    public class LoginController : Controller
    {

        private readonly IUserModel _userModel;

        public LoginController(IUserModel userModel)
        {
            _userModel = userModel;
        }

        // View to Admin Dashboard
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // View to Admin Login
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        // Action for LogIn
        [HttpPost]
        public IActionResult LogIn(UserEnt entity)
        {
            return View();
        }

        // View to Admin Recov Pass
        [HttpGet]
        public IActionResult RecoverPassword()
        {
            return View();
        }

        // View to Admin Change Pass
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // Action to Logout Admin account, exit system
        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

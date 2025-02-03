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
        private readonly IBookModel _bookModel;

        public LoginController(IUserModel userModel, IBookModel bookModel)
        {
            _userModel = userModel;
            _bookModel = bookModel;
        }

        // View to Admin Dashboard
        [HttpGet]
        public IActionResult Index()
        {
            //var data = _bookModel.GetAllBooks().Where(m => m.status_book == true).ToList();
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
            if (!ModelState.IsValid)
                return View();

            var resp = _userModel.LogIn(entity);

            if (resp != null)
            {
                HttpContext.Session.SetString("user_name", resp.full_name);
                HttpContext.Session.SetString("user_token", resp.token);
                HttpContext.Session.SetString("user_role", resp.id_role.ToString());

                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Message = "No se pudo validar su cuenta";
                return View();
            }
        }

        // Action to Logout Admin account, exit system
        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LogIn", "Login");
        }

        // View to Admin Recov Pass
        [HttpGet]
        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecoverPassword(UserEnt entity)
        {
            var resp = _userModel.RecoverPassword(entity);

            if (resp == 1)
                return RedirectToAction("LogIn", "Login");
            else
            {
                ViewBag.Message = "No se pudo validar su cuenta";
                return View();
            }

        }

        // View to Admin Change Pass
        [HttpGet]
        public IActionResult ChangeTempKey(string q)
        {
            UserEnt entity = new UserEnt();
            entity.id_user_auth = q;
            return View(entity);
        }

        [HttpPost]
        public IActionResult ChangeTempKey(UserEnt entity)
        {
            var resp = _userModel.ChangeTempKey(entity);

            if (resp == 1)
                return RedirectToAction("LogIn", "login");
            else
            {
                return View();
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using LMS_WEB.Entities;
using LMS_WEB.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMS_WEB.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorModel _authorModel;

        public AuthorController(IAuthorModel authorModel)
        {
            _authorModel = authorModel;
        }

        [HttpGet]
        public IActionResult ViewAuthors()
        {
            var data = _authorModel.GetAllAuthors();
            return View(data);
        }

        [HttpGet]
        public IActionResult AddAuthor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAuthor(AuthorEnt entity)
        {

            var id_book = _authorModel.AddAuthor(entity);

            if (id_book > 0)
            {

                return RedirectToAction("ViewAuthors", "Author");
            }

            ViewBag.Message = "No se pudo registrar el autor";
            return View();
        }

        [HttpGet]
        public IActionResult UpdateAuthor(long q)
        {
            var data = _authorModel.GetAllAuthors().Where(m => m.id_author == q).FirstOrDefault();

            return View(data);
        }

        [HttpPost]
        public IActionResult UpdateAuthor(AuthorEnt entity)
        {

            var resp = _authorModel.UpdateAuthor(entity);

            if (resp == 1)
            {

                return RedirectToAction("ViewAuthors", "Author");
            }

            ViewBag.Message = "No se pudo actualizar el autor";
            return View();
        }


    }
}

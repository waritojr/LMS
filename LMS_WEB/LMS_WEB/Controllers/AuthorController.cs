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
        public IActionResult AddAuthor(AuthorEnt entity, IFormCollection form)
        {
            // Convertir de string a DateTime (formato dd/MM/yyyy)
            if (DateTime.TryParseExact(form["date_of_birth"], "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None,
                                       out DateTime parsedDate))
            {
                entity.date_of_birth = parsedDate;
            }
            else
            {
                entity.date_of_birth = DateTime.MinValue; // Manejo de error si la fecha es inválida
            }

            // Llamar al modelo para agregar el autor
            var id_author = _authorModel.AddAuthor(entity);

            if (id_author > 0)
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

            if (data == null)
            {
                data = new AuthorEnt(); // Evita que la vista reciba un modelo nulo
            }

            return View(data);
        }

        [HttpPost]
        public IActionResult UpdateAuthor(IFormCollection form)
        {
            var entity = new AuthorEnt
            {
                id_author = Convert.ToInt64(form["id_author"]),
                name_author = form["name_author"],
                nationality = form["nationality"],
                biography = form["biography"]
            };

            // Convertir de string a DateOnly y luego a DateTime
            if (DateTime.TryParseExact(form["date_of_birth"], "dd/MM/yyyy",
                               System.Globalization.CultureInfo.InvariantCulture,
                               System.Globalization.DateTimeStyles.None,
                               out DateTime parsedDate))
            {
                entity.date_of_birth = parsedDate;
            }
            else
            {
                entity.date_of_birth = DateTime.MinValue; // Manejo de error si la fecha es inválida
            }

            var resp = _authorModel.UpdateAuthor(entity);

            if (resp == 1)
            {
                return RedirectToAction("ViewAuthors", "Author");
            }

            ViewBag.Message = "No se pudo actualizar el autor";
            return View(entity);
        }



    }
}

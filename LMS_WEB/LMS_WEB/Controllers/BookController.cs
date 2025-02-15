using LMS_WEB.Entities;
using LMS_WEB.Interfaces;
using LMS_WEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS_WEB.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookModel _bookModel;
        private readonly IAuthorModel _authorModel;
        private IHostEnvironment _hostingEnvironment;

        public BookController(IBookModel bookModel, IAuthorModel authorModel, IHostEnvironment hostingEnvironment)
        {
            _bookModel = bookModel;
            _authorModel = authorModel;
            _hostingEnvironment = hostingEnvironment;
        }

 
        // View to consult books
        [HttpGet]
        public IActionResult ViewBooks()
        {
            var data = _bookModel.GetAllBooks();
            return View(data);
        }

        // View to add books
        [HttpGet]
        public IActionResult AddBook()
        {
            ViewBag.Classification = _bookModel.GetClassificationType();
            ViewBag.Language = _bookModel.GetLanguage();
            ViewBag.Author = _authorModel.ListAuthors();
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(IFormFile img_book, BookEnt entity)
        {
            string ext = Path.GetExtension(Path.GetFileName(img_book.FileName));
            string folder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot\\images");

            if (ext.ToLower() != ".png")
            {
                ViewBag.Message = "La imagen debe ser .png";
                return View();
            }

            var id_book = _bookModel.AddBook(entity);

            if (id_book > 0)
            {
                string file = Path.Combine(folder, id_book + ext);
                using (Stream fileStream = new FileStream(file, FileMode.Create))
                {
                    img_book.CopyTo(fileStream);
                }

                return RedirectToAction("ViewAuthors", "Author");
            }

            ViewBag.Message = "No se pudo registrar el libro";
            ViewBag.Classification = _bookModel.GetClassificationType();
            ViewBag.Language = _bookModel.GetLanguage();
            ViewBag.Author = _authorModel.ListAuthors();
            return View();
        }

        // View to update books
        [HttpGet]
        public IActionResult UpdateBook(long q)
        {
            var data = _bookModel.GetAllBooks().Where(m => m.id_book == q).FirstOrDefault();
            ViewBag.Classification = _bookModel.GetClassificationType();
            ViewBag.Language = _bookModel.GetLanguage();
            ViewBag.Author = _authorModel.ListAuthors();
            return View(data);
        }

        [HttpPost]
        public IActionResult UpdateBook(IFormFile img_book, BookEnt entity)
        {
            string ext = string.Empty;
            string folder = string.Empty;
            string file = string.Empty;

            if (img_book != null)
            {
                ext = Path.GetExtension(Path.GetFileName(img_book.FileName));
                folder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot\\images");
                file = Path.Combine(folder, entity.id_book + ext);

                if (ext.ToLower() != ".png")
                {
                    ViewBag.MensajePantalla = "La imagen debe ser .png";
                    return View();
                }

            }

            var resp = _bookModel.UpdateBook(entity);


            if (resp == 1)
            {
                if (img_book != null)
                {
                    using (Stream fileStream = new FileStream(file, FileMode.Create))
                    {
                        img_book.CopyTo(fileStream);
                    }
                }

                return RedirectToAction("ViewBooks", "Book");
            }
            else
            {
                ViewBag.Message = "No se pudo actualizar el libro";
                ViewBag.Classification = _bookModel.GetClassificationType();
                ViewBag.Language = _bookModel.GetLanguage();
                ViewBag.Author = _authorModel.ListAuthors();
                return View();
            }
        }

        [HttpGet]
        public IActionResult UpdateStatusBook(long q)
        {
            var entity = new BookEnt();
            entity.id_book = q;

            _bookModel.UpdateStatusBook(entity);
            return RedirectToAction("ViewBooks", "Book");
        }

        [HttpGet]
        public IActionResult BookCatalog()
        {
            var data = _bookModel.GetAllBooks();
            return View(data);
        }

        [HttpGet]
        public IActionResult AdvancedSearch()
        {
            // Cargar los SelectListItem para clasificaciones e idiomas
            ViewBag.Classifications = _bookModel.GetClassificationType() ?? new List<SelectListItem>();
            ViewBag.Languages = _bookModel.GetLanguage() ?? new List<SelectListItem>();

            // Cargar las opciones de disponibilidad
            ViewBag.AvailabilityOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "true", Text = "Disponible" },
                    new SelectListItem { Value = "false", Text = "No Disponible" }
                };

            return View(new List<BookEnt>());
        }

        [HttpPost]
        public IActionResult AdvancedSearch(List<BookEnt> filters)
        {
            ViewBag.HasSearched = true;

            ViewBag.Classifications = _bookModel.GetClassificationType() ?? new List<SelectListItem>();
            ViewBag.Languages = _bookModel.GetLanguage() ?? new List<SelectListItem>();

            // Convertir la lista de filtros en parámetros individuales
            string? title = filters.FirstOrDefault(f => f.Field == "title")?.Value;
            string? name_author = filters.FirstOrDefault(f => f.Field == "name_author")?.Value;
            string? isbn = filters.FirstOrDefault(f => f.Field == "isbn")?.Value;
            string? classification_name = filters.FirstOrDefault(f => f.Field == "classification_name")?.Value;
            string? subject_book = filters.FirstOrDefault(f => f.Field == "subject_book")?.Value;
            string? publisher = filters.FirstOrDefault(f => f.Field == "publisher")?.Value;

            // Manejar el rango de fechas
            DateTime? publication_date_from = null;
            DateTime? publication_date_until = null;
            
            var date_filter = filters.FirstOrDefault(f => f.Field == "publication_date");

            if (date_filter != null)
            {
                if (DateTime.TryParse(date_filter.Value, out var from_date))
                {
                    publication_date_from = from_date;
                }

                if (DateTime.TryParse(date_filter.ValueExtra, out var until_date))
                {
                    publication_date_until = until_date;
                }
            }


            // Manejar disponibilidad (bool)
            bool? availability_book = null;
            var availabilityFilter = filters.FirstOrDefault(f => f.Field == "availability_book");
            if (availabilityFilter != null && bool.TryParse(availabilityFilter.Value, out var availability))
            {
                availability_book = availability;
            }

            // Manejar id_classification (long)
            long? id_classification = null;
            var classificationFilter = filters.FirstOrDefault(f => f.Field == "id_classification");
            if (classificationFilter != null && long.TryParse(classificationFilter.Value, out var classificationId))
            {
                id_classification = classificationId;
            }

            // Manejar id_language (long)
            long? id_language = null;
            var languageFilter = filters.FirstOrDefault(f => f.Field == "id_language");
            if (languageFilter != null && long.TryParse(languageFilter.Value, out var languageId))
            {
                id_language = languageId;
            }

            // Llamar al método del modelo para realizar la búsqueda
            var resp = _bookModel.AdvancedSearch(
                title, name_author, isbn, classification_name, subject_book,
                publisher, publication_date_from, publication_date_until,
                availability_book, id_classification, id_language);

            ViewBag.TotalResults = resp?.Count ?? 0;

            // Pasar los resultados a la vista
            return View(resp ?? new List<BookEnt>());
        }


        
    }
}

using LMS_WEB.Entities;
using LMS_WEB.Interfaces;
using LMS_WEB.Models;
using Microsoft.AspNetCore.Mvc;

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
            ViewBag.Classification = _bookModel.GetClassificationType();
            ViewBag.Language = _bookModel.GetLanguage();
            ViewBag.Author = _authorModel.ListAuthors();

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

            ViewBag.Classification = _bookModel.GetClassificationType();
            ViewBag.Language = _bookModel.GetLanguage();
            ViewBag.Author = _authorModel.ListAuthors();

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
                ViewBag.MensajePantalla = "No se pudo actualizar el libro";
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
    }
}

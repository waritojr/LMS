using LMS_WEB.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMS_WEB.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookModel _bookModel;

        public BookController(IBookModel bookModel)
        {
            _bookModel = bookModel;
        }

        // View to consult books
        [HttpGet]
        public IActionResult ViewBooks()
        {
            return View();
        }

        // View to add books
        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }

        // View to update books
        [HttpGet]
        public IActionResult UpdateBook()
        {
            return View();
        }
    }
}

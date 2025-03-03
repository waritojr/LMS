using LMS_WEB.Entities;
using LMS_WEB.Interfaces;
using LMS_WEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

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

        [HttpGet]
        public IActionResult ViewBook(long q)
        {
            var data = _bookModel.GetAllBooks()?.Where(m => m.id_book == q).FirstOrDefault();

            return View(data);
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

            var availabilityOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Disponible", Value = "true" },
                    new SelectListItem { Text = "No Disponible", Value = "false" }
                };

            ViewBag.AvailabilityOptions = availabilityOptions;
            ViewBag.Classification = _bookModel.GetClassificationType();
            ViewBag.Language = _bookModel.GetLanguage();
            ViewBag.Author = _authorModel.ListAuthors();
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(IFormFile img_book, BookEnt entity, IFormCollection form)
        {
            // Convertir de string a DateTime (formato dd/MM/yyyy)
            if (DateTime.TryParseExact(form["publication_date"], "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None,
                                       out DateTime parsedDate))
            {
                entity.publication_date = parsedDate;
            }
            else
            {
                entity.publication_date = DateTime.MinValue; // Manejo de error si la fecha es inválida
            }

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

                return RedirectToAction("ViewBooks", "Books");
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

            if (data == null)
            {
                return NotFound();
            }

            var availabilityOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Disponible", Value = "true" },
                    new SelectListItem { Text = "No Disponible", Value = "false" }
                };

            ViewBag.AvailabilityOptions = availabilityOptions;
            ViewBag.Classification = _bookModel.GetClassificationType();
            ViewBag.Language = _bookModel.GetLanguage();
            ViewBag.Author = _authorModel.ListAuthors();
            return View(data);
        }

        [HttpPost]
        public IActionResult UpdateBook(IFormCollection form, IFormFile img_book)
        {
            var entity = new BookEnt
            {
                id_book = Convert.ToInt64(form["id_book"]),
                title = form["title"],
                classification_name = form["classification_name"],
                id_classification = Convert.ToInt32(form["id_classification"]),
                subject_book = form["subject_book"],
                id_language = Convert.ToInt32(form["id_language"]),
                description_book = form["description_book"],
                isbn = form["isbn"],
                id_author = Convert.ToInt64(form["id_author"]),
                publisher = form["publisher"],
                quantity = Convert.ToInt32(form["quantity"]),
                availability_book = Convert.ToBoolean(form["availability_book"])
                //availability_book = form["availability_book"].ToString() == "on"

            };

            // Conversión de la fecha de publicación de "dd/MM/yyyy" a DateTime
            if (DateTime.TryParseExact(form["publication_date"], "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None,
                                       out DateTime parsedDate))
            {
                entity.publication_date = parsedDate;
            }
            else
            {
                entity.publication_date = DateTime.MinValue; // Manejo de error si la fecha es inválida
            }

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
                ViewBag.AvailabilityOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Disponible", Value = "true" },
                    new SelectListItem { Text = "No Disponible", Value = "false" }
                };

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
        public IActionResult BookCatalog(int page = 1)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var data = _bookModel.GetAllBooks();
            stopwatch.Stop();

            int page_size = 2;
            int total_items = data?.Count ?? 0;
            int total_pages = (int)Math.Ceiling((double)total_items / page_size);

            var paginated_data = data?.Skip((page - 1) * page_size).Take(page_size).ToList();

            ViewBag.TotalResults = total_items;
            ViewBag.SearchTime = stopwatch.Elapsed.TotalSeconds.ToString("0.0000", CultureInfo.InvariantCulture);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = total_pages;
            return View(paginated_data);
        }

        [HttpPost]
        public IActionResult BookCatalog(string search_term, int page = 1)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            List<BookEnt>? data;

            if (string.IsNullOrEmpty(search_term))
            {
                // Si no hay término de búsqueda, mostrar todos los libros
                data = _bookModel.GetAllBooks();
            }
            else
            {
                // Realizar la búsqueda simple
                data = _bookModel.SimpleSearch(search_term);
            }
            stopwatch.Stop();

            int page_size = 2;
            int total_items = data?.Count ?? 0;
            int total_pages = (int)Math.Ceiling((double)total_items / page_size);

            var paginated_data = data?.Skip((page - 1) * page_size).Take(page_size).ToList();

            ViewBag.TotalResults = total_items;
            ViewBag.SearchTime = stopwatch.Elapsed.TotalSeconds.ToString("0.0000", CultureInfo.InvariantCulture);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = total_pages;
            ViewBag.SearchTerm = search_term;
            return View(paginated_data);
        }

        [HttpGet]
        public IActionResult AdvancedSearch(string title = null,
                                            string name_author = null,
                                            string isbn = null,
                                            string classification_name = null,
                                            string subject_book = null,
                                            string publisher = null,
                                            DateTime? publication_date_from = null,
                                            DateTime? publication_date_until = null,
                                            bool? availability_book = null,
                                            long? id_classification = null,
                                            long? id_language = null,
                                            int page = 1)
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

            ViewBag.TitleFilter = title;
            ViewBag.AuthorFilter = name_author;
            ViewBag.IsbnFilter = isbn;
            ViewBag.ClassificationFilter = classification_name;
            ViewBag.SubjectFilter = subject_book;
            ViewBag.PublisherFilter = publisher;
            ViewBag.PublicationDateFromFilter = publication_date_from;
            ViewBag.PublicationDateUntilFilter = publication_date_until;
            ViewBag.AvailabilityFilter = availability_book;
            ViewBag.ClassificationIdFilter = id_classification;
            ViewBag.LanguageIdFilter = id_language;

            var resp = _bookModel.AdvancedSearch(title, name_author, isbn, classification_name, subject_book,
                                                publisher, publication_date_from, publication_date_until,
                                                availability_book, id_classification, id_language);

            int page_size = 2;
            int total_items = resp?.Count ?? 0;
            int total_pages = (int)Math.Ceiling((double)total_items / page_size);

            var paginated_data = resp?.Skip((page - 1) * page_size).Take(page_size).ToList();

            ViewBag.TotalResults = total_items;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = total_pages;

            return View(paginated_data ?? new List<BookEnt>());
        }

        [HttpPost]
        public IActionResult AdvancedSearch(List<BookEnt> filters, int page = 1)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

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

            ViewBag.TitleFilter = title;
            ViewBag.AuthorFilter = name_author;
            ViewBag.IsbnFilter = isbn;
            ViewBag.ClassificationFilter = classification_name;
            ViewBag.SubjectFilter = subject_book;
            ViewBag.PublisherFilter = publisher;
            ViewBag.PublicationDateFromFilter = publication_date_from;
            ViewBag.PublicationDateUntilFilter = publication_date_until;
            ViewBag.AvailabilityFilter = availability_book;
            ViewBag.ClassificationIdFilter = id_classification;
            ViewBag.LanguageIdFilter = id_language;

            // Llamar al método del modelo para realizar la búsqueda
            var resp = _bookModel.AdvancedSearch(
                title, name_author, isbn, classification_name, subject_book,
                publisher, publication_date_from, publication_date_until,
                availability_book, id_classification, id_language);

            stopwatch.Stop();

            int page_size = 2;
            int total_items = resp?.Count ?? 0;
            int total_pages = (int)Math.Ceiling((double)total_items / page_size);

            var paginated_data = resp?.Skip((page - 1) * page_size).Take(page_size).ToList();

            ViewBag.TotalResults = total_items;
            ViewBag.SearchTime = stopwatch.Elapsed.TotalSeconds.ToString("0.0000", CultureInfo.InvariantCulture);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = total_pages;
            return View(paginated_data ?? new List<BookEnt>());

            //return View(resp ?? new List<BookEnt>());
        }



    }
}

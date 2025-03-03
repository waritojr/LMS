using LMS_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Identity;

namespace LMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private string _connection;

        public BookController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("DefaultConnection");
        }

        
        // Method to Get All Books
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllBooks")]
        public IActionResult GetAllBooks()
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Query<BookEnt>("GetAllBooks",
                        new { },
                        commandType: CommandType.StoredProcedure).ToList();

                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // Method to Add Books
        [HttpPost]
        [Authorize]
        [Route("AddBook")]
        public IActionResult AddBook(BookEnt entity)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {

                    var data = context.Query<long>("AddBook",
                        new
                        {
                            entity.classification_name,
                            entity.title,
                            entity.subject_book,
                            entity.publication_date,
                            entity.publisher,
                            entity.description_book,
                            entity.isbn,
                            entity.availability_book,
                            entity.quantity,
                            entity.id_author,
                            entity.id_classification,
                            entity.id_language
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Method to Update a Book
        [HttpPut]
        [Authorize]
        [Route("UpdateBook")]
        public IActionResult UpdateBook(BookEnt entity)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Execute("UpdateBook",
                        new
                        {
                            entity.id_book,
                            entity.classification_name,
                            entity.title,
                            entity.subject_book,
                            entity.publication_date,
                            entity.publisher,
                            entity.description_book,
                            entity.isbn,
                            entity.availability_book,
                            entity.quantity,
                            entity.id_author,
                            entity.id_classification,
                            entity.id_language
                        },
                        commandType: CommandType.StoredProcedure);

                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Method to change the Status of a Book
        [HttpPut]
        [Authorize]
        [Route("UpdateStatusBook")]
        public IActionResult UpdateStatusBook(BookEnt entity)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Execute("UpdateStatusBook",
                        new { entity.id_book },
                        commandType: CommandType.StoredProcedure);

                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // Method to View a SelectListItem of Classification Types
        [HttpGet]
        [AllowAnonymous]
        [Route("GetClassificationType")]
        public IActionResult GetClassificationType()
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Query<SelectListItem>("GetClassificationType",
                        new { },
                        commandType: CommandType.StoredProcedure).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Method to Get a SelectListItem of Languages
        [HttpGet]
        [AllowAnonymous]
        [Route("GetLanguage")]
        public IActionResult GetLanguage()
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Query<SelectListItem>("GetLanguage",
                        new { },
                        commandType: CommandType.StoredProcedure).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("SimpleSearch")]
        public IActionResult SimpleSearch([FromQuery] string search_term)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Query<BookEnt>("SimpleSearch",
                        new { search_term },
                        commandType: CommandType.StoredProcedure).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("AdvancedSearch")]
        public IActionResult AdvancedSearch([FromQuery] string? title = null,
                                            [FromQuery] string? name_author = null,
                                            [FromQuery] string? isbn = null,
                                            [FromQuery] string? classification_name = null,
                                            [FromQuery] string? subject_book = null,
                                            [FromQuery] string? publisher = null,
                                            [FromQuery] DateTime? publication_date_from = null,
                                            [FromQuery] DateTime? publication_date_until = null,
                                            [FromQuery] bool? availability_book = null,
                                            [FromQuery] long? id_classification = null,
                                            [FromQuery] long? id_language = null)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Query<BookEnt>("AdvancedSearch",
                        new
                        {
                            title,
                            name_author,
                            isbn,
                            classification_name,
                            subject_book,
                            publisher,
                            publication_date_from,
                            publication_date_until,
                            availability_book,
                            id_classification,
                            id_language
                        },
                        commandType: CommandType.StoredProcedure).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}

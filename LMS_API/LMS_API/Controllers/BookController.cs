using LMS_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace LMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private string _connection;

        public BookController(IConfiguration configuration, string connection)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("DefaultConnection");
        }

        // Method to Get All Books
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllBooks")]
        public IActionResult GetAllBooks(BookEnt entity)
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
        [AllowAnonymous]
        [Route("AddBook")]
        public IActionResult AddBook(BookEnt entity)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {

                    var data = context.Query<long>("getAllBooks",
                        new
                        {
                            entity.classification_name,
                            entity.title,
                            entity.subject_book,
                            entity.publication_date,
                            entity.publisher,
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
        [AllowAnonymous]
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
                            entity.classification_name,
                            entity.title,
                            entity.subject_book,
                            entity.publication_date,
                            entity.publisher,
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
        [AllowAnonymous]
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
    }
}

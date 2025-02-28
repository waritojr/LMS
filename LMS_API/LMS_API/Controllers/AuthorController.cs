using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using LMS_API.Entities;
using System.Runtime.InteropServices;

namespace LMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private string _connection;

        public AuthorController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("DefaultConnection");
        }

        // Method to Get All Authors
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllAuthors")]
        public IActionResult GetAllAuthors()
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Query<AuthorEnt>("GetAllAuthors",
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

        // Method to Create an Author
        [HttpPost]
        [Authorize]
        [Route("AddAuthor")]
        public IActionResult AddAuthor(AuthorEnt entity)
        {
            try
            {
                using ( var context = new SqlConnection(_connection))
                {
                    var data = context.Query<long>("AddAuthor",
                        new { entity.name_author, entity.nationality, entity.date_of_birth, entity.biography },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Method to Update an Author
        [HttpPut]
        [Authorize]
        [Route("UpdateAuthor")]
        public IActionResult UpdateAuthor(AuthorEnt entity)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Execute("UpdateAuthor",
                        new { entity.id_author, entity.name_author, entity.nationality, entity.date_of_birth, entity.biography },
                        commandType: CommandType.StoredProcedure);

                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Method to Get Authors as a SelectListItem
        [HttpGet]
        [AllowAnonymous]
        [Route("ListAuthors")]
        public IActionResult ListAuthors()
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Query<SelectListItem>("ListAuthors",
                        new {  },
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

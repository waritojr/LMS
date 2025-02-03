using Dapper;
using LMS_API.Entities;
using LMS_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using LMS_API.Models;

namespace LMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IUtilities _utilities;
        private string _connection;

        public UserController(IConfiguration configuration, IUtilities utilities)
        {
            _configuration = configuration;
            _utilities = utilities;
            _connection = _configuration.GetConnectionString("DefaultConnection");
        }

        // Method to Get a User by their ID
        [HttpGet]
        [Authorize]
        [Route("GetUser")]
        public IActionResult GetUser(long q)
        {
            try
            {

                long id_user = (q != 0 ? q : _utilities.GetUserAuth(User.Claims));

                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Query<UserEnt>("GetUser",
                        new { id_user },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return Ok(data);
                }
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        // Method to Update Profile 
        [HttpPut]
        [Authorize]
        [Route("UpdateProfile")]
        public IActionResult UpdateProfile(UserEnt entity)
        {
            try
            {
                long id_user = (entity.id_user != 0 ? entity.id_user : _utilities.GetUserAuth(User.Claims));

                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Execute("UpdateProfile",
                        new { id_user, entity.username, entity.full_name, entity.email, entity.identification, entity.tel },
                        commandType: CommandType.StoredProcedure);

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Method to Update Password inside system
        [HttpPut]
        [Authorize]
        [Route("ChangePassword")]
        public IActionResult ChangePassword(UserEnt entity)
        {
            try
            {

                long id_user = _utilities.GetUserAuth(User.Claims);

                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Execute("ChangePassword",
                        new { id_user, entity.password_prev, entity.password_user },
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

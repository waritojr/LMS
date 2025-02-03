using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using LMS_API.Entities;
using Microsoft.AspNetCore.Authorization;
using LMS_API.Interfaces;

namespace LMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IUtilities _utilities;
        private string _connection;

        public LoginController(IConfiguration configuration, IUtilities utilities)
        {
            _configuration = configuration;
            _utilities = utilities;
            _connection = _configuration.GetConnectionString("DefaultConnection");
        }

        // Method for the Admin LogIn 
        [HttpPost]
        [AllowAnonymous]
        [Route("LogIn")]
        public IActionResult LogIn(UserEnt entity)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Query<UserEnt>("SignIn",
                        new { entity.username, entity.password_user },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (data != null)
                    {
                        data.token = _utilities.GenerateToken(data.id_user.ToString(), data.id_role.ToString());
                        return Ok(data);
                    }
                    else
                    {
                        return BadRequest("No se logró validar su información.");
                    }

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPost]
        [AllowAnonymous]
        [Route("SignUp")]
        public IActionResult SignUp(UserEnt entity)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Execute("SignUp",
                        new { entity.username, entity.full_name, entity.email, entity.identification, entity.tel, entity.password_user },
                        commandType: CommandType.StoredProcedure);

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("RecoverPassword")]
        public IActionResult RecoverPassword(UserEnt entity)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    var data = context.Query<UserEnt>("RecoverPassword",
                        new { entity.username },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (data != null)
                    {
                        string password_temp = _utilities.GenerateCode();
                        string content = _utilities.SetUpHTML(data, password_temp);

                        context.Execute("UpdateTempKey",
                            new { data.id_user, password_temp },
                            commandType: CommandType.StoredProcedure);

                        _utilities.SendEmail(data.email, "Restaurar Contraseña", content);
                        return Ok(1);
                    }
                    else
                        return Ok(0);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("ChangeTempKey")]
        public IActionResult ChangeTempKey(UserEnt entity)
        {
            try
            {
                using (var context = new SqlConnection(_connection))
                {
                    entity.id_user = long.Parse(_utilities.Decrypt(entity.id_user_auth));

                    var datos = context.Execute("ChangeTempKey",
                        new { entity.id_user, entity.password_temp, entity.password_user },
                        commandType: CommandType.StoredProcedure);

                    return Ok(1);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}

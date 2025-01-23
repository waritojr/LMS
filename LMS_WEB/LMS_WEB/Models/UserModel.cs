using LMS_WEB.Interfaces;
using System.Net.Http.Headers;
using LMS_WEB.Entities;

namespace LMS_WEB.Models
{
    public class UserModel : IUserModel
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private string _urlAPI;

        public UserModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlAPI = _configuration.GetSection("Keys:urlAPI").Value;
        }

        public void LogIn()
        {

        }

        public void SignUp()
        {

        }
    }
}

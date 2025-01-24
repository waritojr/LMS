using LMS_WEB.Interfaces;
using System.Net.Http.Headers;
using LMS_WEB.Entities;
using NuGet.Common;

namespace LMS_WEB.Models
{
    public class UserModel : IUserModel
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private string _urlAPI;
        private string _token;

        public UserModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlAPI = _configuration.GetSection("Keys:urlAPI").Value;
            _token = _HttpContextAccessor.HttpContext.Session.GetString("user_token");
        }

        public UserEnt? LogIn(UserEnt entity)
        {
            string url = _urlAPI + "api/Login/LogIn";
            JsonContent obj = JsonContent.Create(entity);
            var resp = _httpClient.PostAsync(url, obj).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<UserEnt>().Result;
            else
                return null;
        }

        public int RecoverPassword(UserEnt entity)
        {
            string url = _urlAPI + "api/Login/RecoverPassword";
            JsonContent obj = JsonContent.Create(entity);
            var resp = _httpClient.PostAsync(url, obj).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<int>().Result;
            else
                return 0;
        }

        public int ChangeTempKey(UserEnt entity)
        {
            string url = _urlAPI + "api/Login/ChangeTempKey";
            JsonContent obj = JsonContent.Create(entity);
            var resp = _httpClient.PostAsync(url, obj).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<int>().Result;
            else
                return 0;
        }

        public UserEnt? GetUser(long q)
        {
            string url = _urlAPI + "api/User/GetUser?q=" + q;
            string token = _HttpContextAccessor.HttpContext.Session.GetString("user_token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);            
            var resp = _httpClient.GetAsync(url).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<UserEnt>().Result;
            else
                return null;
        }

        public int UpdateProfile(UserEnt entity)
        {
            string url = _urlAPI + "api/User/UpdateProfile";
            string token = _HttpContextAccessor.HttpContext.Session.GetString("user_token");

            JsonContent obj = JsonContent.Create(entity);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resp = _httpClient.PutAsync(url, obj).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<int>().Result;
            else
                return 0;

        }

        public int ChangePassword(UserEnt entity)
        {
            string url = _urlAPI + "api/User/ChangePassword";
            string token = _HttpContextAccessor.HttpContext.Session.GetString("user_token");

            JsonContent obj = JsonContent.Create(entity);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resp = _httpClient.PutAsync(url, obj).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<int>().Result;
            else
                return 0;

        }



    }
}

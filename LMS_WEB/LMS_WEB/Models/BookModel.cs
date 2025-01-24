using LMS_WEB.Interfaces;
using LMS_WEB.Entities;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS_WEB.Models
{
    public class BookModel : IBookModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private string _urlAPI;
        private string _token;

        public BookModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlAPI = _configuration.GetSection("Keys:urlAPI").Value;
            _token = _HttpContextAccessor.HttpContext.Session.GetString("user_token");
        }

        public List<BookEnt>? GetAllBooks()
        {
            string url = _urlAPI + "api/Book/GetAllBooks";
            var resp = _httpClient.GetAsync(url).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<List<BookEnt>>().Result;
            else
                return null;
        }

        public int AddBook(BookEnt entity)
        {
            string url = _urlAPI + "api/Book/AddBook";
            string token = _HttpContextAccessor.HttpContext.Session.GetString("user_token");

            JsonContent obj = JsonContent.Create(entity);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resp = _httpClient.PostAsync(url, obj).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<int>().Result;
            else
                return 0;
        }

        public int UpdateBook(BookEnt entity)
        {
            string url = _urlAPI + "api/Book/UpdateBook";
            string token = _HttpContextAccessor.HttpContext.Session.GetString("user_token");

            JsonContent obj = JsonContent.Create(entity);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resp = _httpClient.PutAsync(url, obj).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<int>().Result;
            else
                return 0;
        }

        public int UpdateStatusBook(BookEnt entity)
        {
            string url = _urlAPI + "api/Book/UpdateStatusBook";
            string token = _HttpContextAccessor.HttpContext.Session.GetString("user_token");

            JsonContent obj = JsonContent.Create(entity);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resp = _httpClient.PutAsync(url, obj).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<int>().Result;
            else
                return 0;
        }

        public List<SelectListItem>? GetClassificationType()
        {
            string url = _urlAPI + "api/Book/GetClassificationType";
            string token = _HttpContextAccessor.HttpContext.Session.GetString("user_token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resp = _httpClient.GetAsync(url).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<List<SelectListItem>>().Result;
            else
                return null;
        }
        public List<SelectListItem>? GetLanguage()
        {
            string url = _urlAPI + "api/Book/GetLanguage";
            string token = _HttpContextAccessor.HttpContext.Session.GetString("user_token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resp = _httpClient.GetAsync(url).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<List<SelectListItem>>().Result;
            else
                return null;
        }
    }
}

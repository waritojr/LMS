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

        //public List<BookEnt>? AdvancedSearch(string? title, string? name_author, string? isbn, string? classification_name, string? subject_book)
        //{
        //    try
        //    {
        //        // Construir la URL con los parámetros de búsqueda
        //        var queryParams = new List<string>();
        //        if (!string.IsNullOrEmpty(title)) queryParams.Add($"title={Uri.EscapeDataString(title)}");
        //        if (!string.IsNullOrEmpty(name_author)) queryParams.Add($"name_author={Uri.EscapeDataString(name_author)}");
        //        if (!string.IsNullOrEmpty(isbn)) queryParams.Add($"isbn={Uri.EscapeDataString(isbn)}");
        //        if (!string.IsNullOrEmpty(classification_name)) queryParams.Add($"classification_name={Uri.EscapeDataString(classification_name)}");
        //        if (!string.IsNullOrEmpty(subject_book)) queryParams.Add($"subject_book={Uri.EscapeDataString(subject_book)}");

        //        string url = $"{_urlAPI}api/Book/AdvancedSearch?{string.Join("&", queryParams)}";

        //        // Realizar la solicitud GET a la API
        //        var resp = _httpClient.GetAsync(url).Result;

        //        // Verificar si la respuesta es exitosa
        //        if (resp.IsSuccessStatusCode)
        //            return resp.Content.ReadFromJsonAsync<List<BookEnt>>().Result;
        //        else
        //            return null;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public List<BookEnt>? AdvancedSearch(string? title, string? name_author, string? isbn, string? classification_name, string? subject_book,
                                string? publisher, DateTime? publication_date_from, DateTime? publication_date_until,
                                bool? availability_book, long? id_classification, long? id_language)
        {
            try
            {
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(title)) queryParams.Add($"title={Uri.EscapeDataString(title)}");
                if (!string.IsNullOrEmpty(name_author)) queryParams.Add($"name_author={Uri.EscapeDataString(name_author)}");
                if (!string.IsNullOrEmpty(isbn)) queryParams.Add($"isbn={Uri.EscapeDataString(isbn)}");
                if (!string.IsNullOrEmpty(classification_name)) queryParams.Add($"classification_name={Uri.EscapeDataString(classification_name)}");
                if (!string.IsNullOrEmpty(subject_book)) queryParams.Add($"subject_book={Uri.EscapeDataString(subject_book)}");
                if (!string.IsNullOrEmpty(publisher)) queryParams.Add($"publisher={Uri.EscapeDataString(publisher)}");
                if (publication_date_from.HasValue) queryParams.Add($"publication_date_from={publication_date_from.Value:yyyy-MM-dd}");
                if (publication_date_until.HasValue) queryParams.Add($"publication_date_until={publication_date_until.Value:yyyy-MM-dd}");
                if (availability_book.HasValue) queryParams.Add($"availability_book={availability_book.Value}");
                if (id_classification.HasValue) queryParams.Add($"id_classification={id_classification.Value}");
                if (id_language.HasValue) queryParams.Add($"id_language={id_language.Value}");

                string url = $"{_urlAPI}api/Book/AdvancedSearch?{string.Join("&", queryParams)}";
                var resp = _httpClient.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                    return resp.Content.ReadFromJsonAsync<List<BookEnt>>().Result;
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

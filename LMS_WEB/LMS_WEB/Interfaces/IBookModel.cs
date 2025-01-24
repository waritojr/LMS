using LMS_WEB.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS_WEB.Interfaces
{
    public interface IBookModel
    {
        public List<BookEnt>? GetAllBooks();

        public int AddBook(BookEnt entity);

        public int UpdateBook(BookEnt entity);

        public int UpdateStatusBook(BookEnt entity);

        public List<SelectListItem>? GetClassificationType();

        public List<SelectListItem>? GetLanguage();
    }
}

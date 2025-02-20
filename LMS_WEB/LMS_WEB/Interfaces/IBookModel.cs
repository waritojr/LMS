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
        public List<BookEnt>? SimpleSearch(string search_term);

        public List<BookEnt>? AdvancedSearch(string? title, string? name_author, string? isbn, string? classification_name, string? subject_book,
                                        string? publisher, DateTime? publication_date_from, DateTime? publication_date_until,
                                        bool? availability_book, long? id_classification, long? id_language);
    }
}

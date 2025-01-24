using LMS_WEB.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS_WEB.Interfaces
{
    public interface IAuthorModel
    {
        public List<AuthorEnt>? GetAllAuthors();

        public int AddAuthor(BookEnt entity);

        public int UpdateAuthor(BookEnt entity);

        public List<SelectListItem>? ListAuthors();
    }
}

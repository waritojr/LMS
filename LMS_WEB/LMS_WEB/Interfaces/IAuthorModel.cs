using LMS_WEB.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS_WEB.Interfaces
{
    public interface IAuthorModel
    {
        public List<AuthorEnt>? GetAllAuthors();

        public int AddAuthor(AuthorEnt entity);

        public int UpdateAuthor(AuthorEnt entity);

        public List<SelectListItem>? ListAuthors();
    }
}

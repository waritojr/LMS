namespace LMS_WEB.Entities
{
    public class AuthorEnt
    {
        public long id_author { get; set; }
        public string name_author { get; set; } = string.Empty;
        public string nationality { get; set; } = string.Empty;
        public DateTime date_of_birth { get; set; }
        public string biography { get; set; } = string.Empty;
    }
}

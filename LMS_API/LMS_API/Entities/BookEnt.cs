namespace LMS_API.Entities
{
    public class BookEnt
    {
        public long id_book { get; set; }
        public string classification_name { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string subject_book { get; set; } = string.Empty;
        public DateTime publication_date { get; set; }
        public string publisher { get; set; } = string.Empty;
        public string description_book { get; set; } = string.Empty;
        public string isbn { get; set; } = string.Empty;
        public bool availability_book { get; set; }
        public int quantity { get; set; }
        public bool status_book { get; set; }
        public string img_book { get; set; } = string.Empty;


        // Search Variables
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public DateTime publication_date_from { get; set; }
        public DateTime publication_date_until { get; set; }


        // FK
        public long id_author { get; set; }
        public string name_author { get; set; } = string.Empty;
        public long id_language { get; set; }
        public string language_name { get; set; } = string.Empty;
        public long id_classification { get; set; }
        public string classification_type { get; set; } = string.Empty;

    }
}

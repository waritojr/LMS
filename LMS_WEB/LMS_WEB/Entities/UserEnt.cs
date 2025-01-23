namespace LMS_WEB.Entities
{
    public class UserEnt
    {
        public long id_user { get; set; }
        public string username { get; set; } = string.Empty;
        public string full_name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string identification { get; set; } = string.Empty;
        public string tel { get; set; } = string.Empty;
        public string password_user { get; set; } = string.Empty;
        public bool status_user { get; set; }
        public DateTime created_at { get; set; }

        // FK
        public long id_role { get; set; }

        // Utilities
        public string token { get; set; } = string.Empty;
        public string id_user_auth { get; set; } = string.Empty;
        public string password_temp { get; set; } = string.Empty;
        public string password_prev { get; set; } = string.Empty;
    }
}

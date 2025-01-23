namespace LMS_WEB.Entities
{
    public class ReservationEnt
    {
        public long id_reservation { get; set; }
        public string name_rsv { get; set; } = string.Empty;
        public string email_rsv { get; set; } = string.Empty;
        public string tel_rsv { get; set; } = string.Empty;
        public DateTime date_rsv { get; set; }
        public bool status_rsv { get; set; }
        public DateTime created_at { get; set; }

        // FK
        public long id_book { get; set; }
    }
}

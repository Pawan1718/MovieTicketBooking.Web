namespace MovieTicketBooking.Web.ViewModels.HomeViewModels
{
    public class ShowtimeDetailsViewModel
    {
        public int ShowtimeId { get; set; }
        public DateTime DateTime { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string? Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string Genre { get; set; }
        public string Image { get; set; }
        public string? TheaterName { get; set; }
        public string? Location { get; set; }
    }
}

namespace MovieTicketBooking.Web.ViewModels.ShowtimeViewModels
{
    public class AddShowtimeViewModel
    {
        public DateTime DateTime { get; set; }
        public decimal Price { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
    }
}

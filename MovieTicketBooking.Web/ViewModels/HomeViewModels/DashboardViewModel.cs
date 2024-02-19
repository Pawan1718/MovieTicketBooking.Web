using MovieTicketBooking.Web.ViewModels.MovieViewModels;

namespace MovieTicketBooking.Web.ViewModels.HomeViewModels
{
    public class DashboardViewModel
    {

        public int ShowtimeId { get; set; }
        public string MovieName { get; set; }
        public string Genre { get; set; }
        public string Image { get; set; }
    }
}

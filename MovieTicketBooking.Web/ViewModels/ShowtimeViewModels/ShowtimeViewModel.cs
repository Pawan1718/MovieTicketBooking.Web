using MovieTicketBooking.Entities.Models;
using MovieTicketBooking.Web.Utility;

namespace MovieTicketBooking.Web.ViewModels.ShowtimeViewModels
{
    public class ShowtimeViewModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Price { get; set; }
        public string MovieName { get; set; }
        public string TheaterName { get; set; }
    }

    public class PagedShowtimeViewModel
    {
        public List<ShowtimeViewModel> Showtimes { get; set; }
        public PageInfo pageInfo { get; set; }
    }
}

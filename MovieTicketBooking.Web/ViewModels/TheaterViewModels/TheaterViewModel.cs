using MovieTicketBooking.Web.Utility;

namespace MovieTicketBooking.Web.ViewModels.TheaterViewModels
{
    public class TheaterViewModel
    {
        public int Id { get; set; }
        public string? TheaterName { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }
    }

    public class PagedTheaterViewModel
    {
        public List<TheaterViewModel>Theaters { get; set; }
        public PageInfo PageInfo{ get; set; }
    }
}

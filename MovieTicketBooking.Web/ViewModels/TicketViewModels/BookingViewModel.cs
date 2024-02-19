namespace MovieTicketBooking.Web.ViewModels.TicketViewModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime Showtime { get; set; }
        public string MovieName { get; set; }
        public string TheaterName { get; set; }
        public string Address { get; set; }
        public List<MyTicketViewModel> Tickets { get; set; }

    }
}

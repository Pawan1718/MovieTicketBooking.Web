
namespace MovieTicketBooking.Web.ViewModels.HomeViewModels
{
    public class AvailableTicketViewModel
    {
        public int ShowtimeId { get; set; }
        public int SeatCapacity { get; set; }
        public List<int>AvailableSeats { get; set; }

        //public List<int> BlockASeats { get; set; }
        //public List<int> BlockBSeats { get; set; }
        //public List<int> BlockCSeats { get; set; }
        //public List<int> BlockDSeats { get; set; }

    }
}

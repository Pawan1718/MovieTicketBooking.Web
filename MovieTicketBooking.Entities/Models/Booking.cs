using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Entities.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime BookingDate{ get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public decimal TotalPrice { get; set; }
        public int ShowtimeId { get; set; }
        public ShowTime? Showtime { get; set; }
        public ICollection<Ticket>Tickets { get; set; }=new List<Ticket>();

    }
}

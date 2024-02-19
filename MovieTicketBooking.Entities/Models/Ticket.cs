using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Entities.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int SeatNo { get; set; }
        public bool isBooked { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}

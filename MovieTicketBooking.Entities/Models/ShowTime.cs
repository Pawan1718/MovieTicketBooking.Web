using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Entities.Models
{
    public class ShowTime
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Price { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int TheaterId { get; set; }
        public Theater Theater { get; set; }

        public ICollection<Booking> Bookings { get; set; }=new List<Booking>();


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Entities.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Director { get; set; }
        public int Duration { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<ShowTime> Showtimes { get; set; } = new List<ShowTime>();
    }
}

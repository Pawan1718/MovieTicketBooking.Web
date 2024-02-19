using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Entities.Models
{
    public class Theater
    {
        public int Id { get; set; }
        public string? TheaterName { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }

        public ICollection<Theater> Theaters { get; set; } = new List<Theater>();
    }
}

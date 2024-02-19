using MovieTicketBooking.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public  interface ITicketRepo
    {
        Task<IEnumerable<int>> GetBookedTickets(int id);
        Task<IEnumerable<Booking>> GetBookings(string userId);
    }
}

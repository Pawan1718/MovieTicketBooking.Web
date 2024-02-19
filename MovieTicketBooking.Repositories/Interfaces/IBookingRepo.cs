using MovieTicketBooking.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IBookingRepo
    {
        Task<IEnumerable<Booking>> GetAll(int showtimeId);
        Task<Booking>GetById(string id);
        Task Save(Booking booking);
        Task Edit(Booking booking);
        Task Remove(Booking booking);
    }
}

using MovieTicketBooking.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IShowtimeRepo
    {
        Task<IEnumerable<ShowTime>> GetAll();
        Task<ShowTime> GetById(int id);
        Task Save(ShowTime showtime);
        Task Edit(ShowTime showtime);
        Task Remove(ShowTime showtime);
    }
}

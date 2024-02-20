using MovieTicketBooking.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface ITheaterRepo
    {
        Task<IEnumerable<Theater>> GetAll();
        Task<Theater> GetById(int id);
        Task Save(Theater theater);
        Task Edit(Theater theater);
        Task Remove(Theater theater);
        Task<bool> isTheaterExist(string theaterName);
        Task<bool> isMovieExist(int id);
        Task<bool> isTheaterWithShowtimesExist(int theaterId);

    }
}

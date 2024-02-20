using MovieTicketBooking.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IMovieRepo
    {
        Task<IEnumerable<Movie>> GetAll();
        Task<Movie> GetById(int id);
        Task Save(Movie movie);
        Task Edit(Movie movie);
        Task Remove(Movie movie);
        Task<bool> isMovieExist(string movie);
        Task<bool> isShowtimeExist(int id);
        Task<bool> isMovieWithTheaterExist(int movieId);

    }
}

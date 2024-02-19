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
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieTicketBooking.Entities.Data;
using MovieTicketBooking.Entities.Models;
using MovieTicketBooking.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repositories.Implimentations
{
    public class MovieRepo : IMovieRepo
    {
        private readonly ApplicationDbContext context;

        public MovieRepo(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task Edit(Movie movie)
        {
            context.Movies.Update(movie);
            await context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await context.Movies.ToListAsync();

        }

        public async Task<Movie> GetById(int id)
        {
            return await context.Movies.FindAsync(id);
        }

        public async Task<bool> isMovieExist(string movie)
        {
            return await context.Movies.AnyAsync(x => x.Title == movie);
        }

        public async Task<bool> isMovieWithTheaterExist(int movieId)
        {
            return await context.Theaters.AnyAsync(x=>x.Id == movieId);
        }

        public async Task<bool> isShowtimeExist(int id)
        {
            return await context.ShowTimes.AnyAsync(x=>x.Id == id);
        }

        public async Task Remove(Movie movie)
        {
            context.Movies.Remove(movie);
            await context.SaveChangesAsync();

        }

        public async Task Save(Movie movie)
        {
            await context.Movies.AddAsync(movie);
            await context.SaveChangesAsync();

        }
    }
}

using Microsoft.EntityFrameworkCore;
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
    public class TheaterRepo:ITheaterRepo
    {
        private readonly ApplicationDbContext context;

        public TheaterRepo(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task Edit(Theater theater)
        {
            context.Theaters.Update(theater);
            await context.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<Theater>> GetAll()
        {
            return await context.Theaters.ToListAsync();
        }

        public async Task<Theater> GetById(int id)
        {
            return await context.Theaters.FindAsync(id);
        }

        public async Task<bool> isMovieExist(int id)
        {
            return await context.Movies.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> isTheaterExist(string theaterName)
        {
            return await context.Theaters.AnyAsync(x => x.TheaterName == theaterName);
        }

        public async Task<bool> isTheaterWithShowtimesExist(int theaterId)
        {
            return await context.ShowTimes.AnyAsync(x => x.TheaterId == theaterId);
        }

        public async Task Remove(Theater theater)
        {
             context.Theaters.Remove(theater);
            await context.SaveChangesAsync();
           
        }

        public async Task Save(Theater theater)
        {
           await context.Theaters.AddAsync(theater);
            await context.SaveChangesAsync();
            
        }
    }
}

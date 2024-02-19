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
    public class ShowtimeRepo : IShowtimeRepo
    {
        private readonly ApplicationDbContext context;

        public ShowtimeRepo(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task Edit(ShowTime showtime)
        {
            context.ShowTimes.Update(showtime);
            await context.SaveChangesAsync();
           
        }

        public async Task<IEnumerable<ShowTime>> GetAll()
        {
            return await context.ShowTimes
                .Include(m=>m.Movie)
                .Include(t=>t.Theater)
                .ToListAsync();
        }

        public async Task<ShowTime> GetById(int id)
        {
            return await context.ShowTimes
                .Include(t=>t.Theater)
                .Include(m=>m.Movie)
                .FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task Remove(ShowTime showtime)
        {
            context.ShowTimes.Remove(showtime);
            await context.SaveChangesAsync();
            
        }

        public async Task Save(ShowTime showtime)
        {
            await context.ShowTimes.AddAsync(showtime);
            await context.SaveChangesAsync();
          
        }
    }
}

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
    public class BookingRepo : IBookingRepo
    {
        private readonly ApplicationDbContext context;

        public BookingRepo(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task Edit(Booking booking)
        {
            context.Bookings.Update(booking);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetAll(int showtimeId)
        {
            return await context.Bookings
                .Include(x=>x.Tickets)
                .Include(x=>x.User)
                .Where(x=>x.ShowtimeId==showtimeId)
                .ToListAsync();
        }

        public async Task<Booking> GetById(string id)
        {
            return await context.Bookings.FindAsync(id);
        }

        public async Task Remove(Booking booking)
        {
            context.Bookings.Remove(booking);
         await context.SaveChangesAsync();
        }

        public async Task Save(Booking booking)
        {
            await context.Bookings.AddAsync(booking);
          await context.SaveChangesAsync();

        }

    }
}

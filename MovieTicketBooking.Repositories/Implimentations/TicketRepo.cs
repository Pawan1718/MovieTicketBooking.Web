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
    public class TicketRepo : ITicketRepo
    {
        private readonly ApplicationDbContext context;

        public TicketRepo(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<int>> GetBookedTickets(int id)
        {
            var bookedTickets = await context.Tickets
                .Include(y=>y.Booking)
                .Where(t => t.Booking.ShowtimeId == id && t.isBooked)
                .Select(t => t.SeatNo).ToListAsync();
            return bookedTickets;
        }

        public async Task<IEnumerable<Booking>> GetBookings(string userId)
        {
            var booking = await context.Bookings.Where(x=>x.UserId == userId)
                .Include(x=>x.Showtime.Movie)
                .Include(x=>x.Tickets)
                .Include(x=>x.Showtime)
                .ToListAsync();
            return booking;
        }
    }
}

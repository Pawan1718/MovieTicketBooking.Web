using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Entities.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Web.ViewModels.TicketViewModels;
using System.Security.Claims;

namespace MovieTicketBooking.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepo ticketRepo;

        public TicketController(ITicketRepo _ticketRepo)
        {
            ticketRepo = _ticketRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> MyTickets()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            var bookings = await ticketRepo.GetBookings (userId); 
            List<BookingViewModel> vm=new List<BookingViewModel>();
            foreach (var booking in bookings)
            {
                var movieTitle = booking.Showtime.Movie?.Title ?? "Unknown Movie";
                var Theater = booking.Showtime.Theater?.TheaterName ?? "Unknown";
                var Address = booking.Showtime.Theater?.Location ?? "Unknown";

                vm.Add(new BookingViewModel
                {
                    BookingId = booking.BookingId,
                    BookingDate = booking.BookingDate,
                    Showtime=booking.Showtime.DateTime,
                    MovieName=movieTitle,
                    TheaterName=Theater,
                    Address=Address,
                    Tickets = booking.Tickets.Select(ticketVM => new MyTicketViewModel
                    {
                        SeatNo = ticketVM.SeatNo,
                    }).ToList()
                });
              }
            return View(vm);
        }
    }
}

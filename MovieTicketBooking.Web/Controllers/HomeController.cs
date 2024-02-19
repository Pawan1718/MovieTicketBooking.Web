using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using MovieTicketBooking.Entities.Models;
using MovieTicketBooking.Repositories.Implimentations;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Web.Models;
using MovieTicketBooking.Web.Utility;
using MovieTicketBooking.Web.ViewModels.HomeViewModels;
using System;
using System.Diagnostics;
using System.Security.Claims;

namespace MovieTicketBooking.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly  IShowtimeRepo _showtimeRepo;
        private readonly  IMovieRepo _movieRepo;
        private readonly  ITheaterRepo _theaterRepo;
        private readonly  ITicketRepo _ticketRepo;
        private readonly  IBookingRepo _bookingRepo;

        public HomeController(ILogger<HomeController> logger, IShowtimeRepo showtimeRepo, IMovieRepo movieRepo, ITheaterRepo theaterRepo, ITicketRepo ticketRepo, IBookingRepo bookingRepo)
        {
            _logger = logger;
            _showtimeRepo = showtimeRepo;
            _movieRepo = movieRepo;
            _theaterRepo = theaterRepo;
            _ticketRepo = ticketRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<IActionResult> Index(string filterText,string SearchText = null)
        {
            DateTime today = DateTime.Today;
            var showtimes = await _showtimeRepo.GetAll();
            if (!string.IsNullOrEmpty(SearchText))
            {
                showtimes = showtimes.Where(x => x.Movie.Title.Contains(SearchText)
                || x.Movie.Genre.Contains(SearchText)).ToList();
            }
            var vm = showtimes
                .Where(s => s.DateTime.Date >= today)
                .Select(showtime => new DashboardViewModel
                {
                    ShowtimeId = showtime.Id,
                    Image = showtime.Movie.ImageUrl,
                    MovieName = showtime.Movie.Title,
                    Genre = showtime.Movie.Genre
                })
                .ToList();
            return View(vm);
        }
        public async Task<IActionResult> ShowtimeDetails(int id)
        {
            var showtime = await _showtimeRepo.GetById(id);

            if (showtime == null)
            {
                return NotFound(); 
            }

            var vm = new ShowtimeDetailsViewModel
            {
                ShowtimeId = showtime.Id,
                DateTime=showtime.DateTime,
                Image = showtime.Movie.ImageUrl,
                MovieName = showtime.Movie.Title,
                Genre = showtime.Movie.Genre,
                Description = showtime.Movie.Description,
                ReleaseDate = showtime.Movie.ReleaseDate,
                Director = showtime.Movie.Director,
                TheaterName = showtime.Theater.TheaterName,
                Location = showtime.Theater.Location,
                Duration = showtime.Movie.Duration,
            };

            return View(vm);
        }
        [Authorize]
        public async Task<IActionResult> AvailableTickets(int id)
        {
            var showtime=await _showtimeRepo.GetById(id);
            if (showtime == null)
            {
                return NotFound();
            }
            var allSeats= Enumerable.Range(1,showtime.Theater.Capacity).ToList();
            var bookedTicket=await _ticketRepo.GetBookedTickets(showtime.Id);
            var availableSeats= allSeats.Except(bookedTicket).ToList();

            var viewModel = new AvailableTicketViewModel
            {
                ShowtimeId = showtime.Id,
                SeatCapacity = showtime.Theater.Capacity,
                AvailableSeats = availableSeats,
                //BlockASeats = availableSeats.Take(seatsPerBlock).ToList(),
                //BlockBSeats = availableSeats.Skip(seatsPerBlock).Take(seatsPerBlock).ToList(),
                //BlockCSeats = availableSeats.Skip(seatsPerBlock * 2).Take(seatsPerBlock).ToList(),
                //BlockDSeats = availableSeats.Skip(seatsPerBlock * 3).Take(seatsPerBlock).ToList()
            };
            return View(viewModel);
        }
        public async Task<IActionResult> BookTickets(int ShowtimeId, List<int> selectedSeats)
        {
            if (selectedSeats == null || selectedSeats.Count == 0)
            {
                ModelState.AddModelError("", "No Seat Selected");
                return RedirectToAction("AvailableTickets", new { id = ShowtimeId });
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            var newBooking = new Booking
            {
                ShowtimeId = ShowtimeId,
                BookingDate = DateTime.Now,
                UserId = userId,
                Showtime = await _showtimeRepo.GetById(ShowtimeId)
            };

            if (newBooking.Showtime == null)
            {
                return NotFound();
            }

            newBooking.Tickets = new List<Ticket>();
            foreach (var seatNo in selectedSeats)
                newBooking.Tickets.Add(new Ticket
                {
                    SeatNo = seatNo,
                    isBooked = true,
                });
            await _bookingRepo.Save(newBooking);

            return RedirectToAction("MyTickets");
        }
       
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using MovieTicketBooking.Entities.Models;
using MovieTicketBooking.Repositories.Implimentations;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Web.ViewModels.DashboardViewModel;
using MovieTicketBooking.Web.ViewModels.MovieViewModels;
using MovieTicketBooking.Web.ViewModels.ShowtimeViewModels;
using MovieTicketBooking.Web.ViewModels.TheaterViewModels;

namespace MovieTicketBooking.Web.Controllers
{
    public class ShowTimeController : Controller
    {
        private readonly IShowtimeRepo showtimeRepo;
        private readonly IMovieRepo movieRepo;
        private readonly ITheaterRepo theaterRepo;
        private readonly IBookingRepo bookingRepo;
        public ShowTimeController(IShowtimeRepo showtimeRepo, IMovieRepo movieRepo, ITheaterRepo theaterRepo, IBookingRepo bookingRepo)
        {
            this.showtimeRepo = showtimeRepo;
            this.movieRepo = movieRepo;
            this.theaterRepo = theaterRepo;
            this.bookingRepo = bookingRepo;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Showtime(string filterText, int PageNo=1, int PageSize=3, string SearchText = null)
        {
            var vm = new List<ShowtimeViewModel>();
            var showtimeList = await showtimeRepo.GetAll();
            if (SearchText!=null)
            {
                PageNo = 1;
            }
            else
            {
                SearchText = filterText;
            }
            ViewData["filterData"] = SearchText;
            int totalItems = 0;

            if (!string.IsNullOrEmpty(SearchText))
            {
                showtimeList = showtimeList.Where(x => 
                x.Movie.Title.Contains(SearchText) 
                || x.Theater.TheaterName.Contains(SearchText) 
                || x.Price.ToString().Contains(SearchText) 
                || x.DateTime.ToString().Contains(SearchText) 
                || x.Movie.Genre.Contains(SearchText)).ToList();
            }
            totalItems = showtimeList.ToList().Count();
            showtimeList = showtimeList.Skip((PageNo - 1) * PageSize).Take(PageSize).ToList();
            foreach (var showtime in showtimeList)
            {
                vm.Add(new ShowtimeViewModel
                {
                    Id = showtime.Id,
                    DateTime = showtime.DateTime,
                    MovieName = showtime.Movie.Title,
                    Price = showtime.Price,
                    TheaterName = showtime.Theater.TheaterName,
                });
            }
            var pvm = new PagedShowtimeViewModel
            {
                Showtimes = vm,
                pageInfo = new Utility.PageInfo
                {
                    PageNo=PageNo,
                    PageSize=PageSize,
                    TotalItems=totalItems
                }
            };
            return View(pvm); 
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddShowtime()
        {
            var movieList = await movieRepo.GetAll();
            var theaterList = await theaterRepo.GetAll();
            ViewBag.MovieList = new SelectList(movieList, "Id", "Title");
            ViewBag.TheaterList = new SelectList(theaterList, "Id", "TheaterName");
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddShowtime(AddShowtimeViewModel vm)
        {
            var showtime = new ShowTime
            {
                DateTime = vm.DateTime,
                TheaterId= vm.TheaterId,
                MovieId = vm.MovieId,
                Price = vm.Price,
            };
            await showtimeRepo.Save(showtime);
            return RedirectToAction("Showtime");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditShowtime(int id)
        {
            var show = await showtimeRepo.GetById(id);

            var movieList = await movieRepo.GetAll();
            var theaterList = await theaterRepo.GetAll();
            ViewBag.MovieList = new SelectList(movieList, "Id", "Title");
            ViewBag.TheaterList = new SelectList(theaterList, "Id", "TheaterName");

            if (show == null)
            {
                return NotFound();
            }
            EditShowtimeViewModel vm = new EditShowtimeViewModel
            {
                Id = show.Id,
                DateTime = show.DateTime,
                MovieId = show.MovieId,
                TheaterId = show.TheaterId,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditShowtime(EditShowtimeViewModel vm)
        {
            var show = new ShowTime
            {
                Id = vm.Id,
                DateTime = vm.DateTime,
                MovieId = vm.MovieId,
                TheaterId = vm.TheaterId,
                Price = vm.Price,
            };
            await showtimeRepo.Edit(show);
            return RedirectToAction("Showtime");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveShowtime(int id)
        {
            var show = await showtimeRepo.GetById(id);
            RemoveShowtimeViewModel vm = new RemoveShowtimeViewModel
            {
                Id = show.Id,
                Price=show.Price,
                TheaterId = show.TheaterId,
                MovieId = show.MovieId,
                DateTime = show.DateTime,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveTheater(RemoveShowtimeViewModel vm)
        {
            var show = new ShowTime
            {
                Id = vm.Id,
                MovieId = vm.MovieId,
                TheaterId = vm.TheaterId,
                Price = vm.Price,

            };
            await showtimeRepo.Remove(show);
            return RedirectToAction("Showtime");
        }
      public async Task<IActionResult> GetBookedTickets(int id)
{
    var bookings = await bookingRepo.GetAll(id);
    var vm = bookings.Select(b => new DashboardBookingViewModel
    {
        UserName = b.User?.Name,
        ShowTime = b.Showtime?.DateTime ?? DateTime.MinValue,
        SeatNo = string.Join(",", b.Tickets.Select(t => t.SeatNo))
    }).ToList();
    
    return View(vm);
}

    }
}

using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Entities.Models;
using MovieTicketBooking.Repositories.Implimentations;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Web.ViewModels.MovieViewModels;
using System.Drawing;
using System.IO;

namespace MovieTicketBooking.Web.Controllers
{
    [Authorize(Roles ="Admin")]

    public class MovieController : Controller
    {
        private readonly IMovieRepo movieRepo;
        private readonly IUtilityRepo UtilityRepo;
        private string containerName = "Movies Poster";
        public MovieController(IMovieRepo _movieRepo, IUtilityRepo utilityRepo)
        {
            movieRepo = _movieRepo;
            UtilityRepo = utilityRepo;
        }
        public async Task<IActionResult> MovieList(string FilterText,int pageNo = 1, int pageSize = 3, string SearchText=null )
        {
         List<MovieViewModel> vm = new List<MovieViewModel>();
            var movies = await movieRepo.GetAll();
            if (SearchText!=null)
            {
                pageNo = 1;
            }
            else
            {
                SearchText = FilterText;
            }
            ViewData["filterData"] = SearchText;

            int totalItems = 0;
            if (!string.IsNullOrEmpty(SearchText))
            {
                movies = movies.Where(x => x.Title.Contains(SearchText) || x.Genre.Contains(SearchText)).ToList();
            }
            totalItems = movies.ToList().Count();
            movies = movies.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            foreach (var movie in movies)
            {
                 vm.Add(new MovieViewModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Description = movie.Description,
                    Genre = movie.Genre,
                    Director = movie.Director,
                    ReleaseDate = movie.ReleaseDate,
                    ImageUrl = movie.ImageUrl,
                    Duration = movie.Duration
                });
            }
            var pvm = new PagedMovieViewModel
            { 
                Movies = vm,
                PageInfo = new Utility.PageInfo
                {
                    PageNo = pageNo,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };
            return View(pvm);
        }
        public async Task<IActionResult> AddMovie()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieViewModel vm)
        {
            var movie = new Movie
            {
                Title = vm.Title,
                Description = vm.Description,
                Genre = vm.Genre,
                Director = vm.Director,
                Duration = vm.Duration,
                ReleaseDate = vm.ReleaseDate,
            };
            if (vm.ImageUrl != null)
            {
                movie.ImageUrl = await UtilityRepo.SaveImage(containerName, vm.ImageUrl);
            }
            await movieRepo.Save(movie);
            return RedirectToAction("MovieList");
        }
        public async Task<IActionResult> EditMovie(int id)
        {
            var movie = await movieRepo.GetById(id);
            EditMovieViewModel vm = new EditMovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                Director = movie.Director,
                Duration = movie.Duration,
                ReleaseDate = movie.ReleaseDate,
                ImageUrl= movie.ImageUrl
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditMovie(EditMovieViewModel vm)
        {
            var movie = await movieRepo.GetById(vm.Id);
            if (movie == null)
            {
                return NotFound();
            }
            movie.Title = vm.Title;
            movie.Description = vm.Description;
            movie.Genre = vm.Genre;
            movie.Director = vm.Director;
            movie.Duration = vm.Duration;
            movie.ReleaseDate = vm.ReleaseDate;

            if (vm.ImageUrl != null)
            {
                movie.ImageUrl = await UtilityRepo.EditImage(containerName, vm.ChooseImage, movie.ImageUrl);
            }
            await movieRepo.Edit(movie);
            return RedirectToAction("MovieList");
        }
        public async Task<IActionResult> RemoveMovie(MovieViewModel vm)
        {
            var movie = new Movie
            {
                Id = vm.Id,
                Title = vm.Title,
                Description = vm.Description,
                Genre = vm.Genre,
                Director = vm.Director,
                Duration = vm.Duration,
                ReleaseDate = vm.ReleaseDate,
            };
            await movieRepo.Remove(movie);
            return RedirectToAction("TheaterList");
        }

    }
}

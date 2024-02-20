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
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> MovieList(string FilterText, int pageNo = 1, int pageSize = 3, string SearchText = null)
        {
            List<MovieViewModel> vm = new List<MovieViewModel>();
            var movies = await movieRepo.GetAll();
            if (SearchText != null)
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
            if (ModelState.IsValid)
            {
                try
                {
                    if (await movieRepo.isMovieExist(vm.Title))
                    {
                        TempData["error"] = vm.Title + " " + "already exist in database!";
                        return View(vm);

                    }
                    else
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
                        TempData["success"] = vm.Title + " " + "Movie added successfully!";
                        return View(vm);
                    }

                }
                catch (Exception ex)
                {
                    TempData["error"] = "Failed to add movie. " + ex.Message;
                }
            }
            else
            {
                TempData["error"] = "Failed to add movie. Please check the input data.";
            }

            return View(vm);
        }
        public async Task<IActionResult> EditMovie(int id)
        {
            try
            {
                var movie = await movieRepo.GetById(id);
                if (movie == null)
                {
                    TempData["error"] = "Movie not found.";
                    return RedirectToAction("Index");
                }

                EditMovieViewModel vm = new EditMovieViewModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Description = movie.Description,
                    Genre = movie.Genre,
                    Director = movie.Director,
                    Duration = movie.Duration,
                    ReleaseDate = movie.ReleaseDate,
                    ImageUrl = movie.ImageUrl
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while loading movie details.";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditMovie(EditMovieViewModel vm)
        {
            try
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
                TempData["success"] = "Movie updated successfully!";
                return RedirectToAction("MovieList");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Failed to update movie. " + ex.Message;
                return RedirectToAction("MovieList");
            }
        }
        public async Task<IActionResult> RemoveMovie(int id)
        {
            var movie = await movieRepo.GetById(id);
            if (movie == null)
            {
                TempData["error"] = "Movie not found.";
                return RedirectToAction("MovieList");
            }

            MovieViewModel vm = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                Director = movie.Director,
                Duration = movie.Duration,
                ReleaseDate = movie.ReleaseDate,
                ImageUrl = movie.ImageUrl,
            };

            bool hasAssociated = await movieRepo.isShowtimeExist(vm.Id) || await movieRepo.isMovieWithTheaterExist(vm.Id);
            if (hasAssociated)
            {
                TempData["warning"] = "Cannot delete the movie because associated showtimes exist.";
            }
            return View(vm);
        }
        [HttpPost]
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
                ImageUrl = vm.ImageUrl,
            };

            bool hasAssociated = await movieRepo.isShowtimeExist(vm.Id) || await movieRepo.isMovieWithTheaterExist(vm.Id);
            if (hasAssociated)
            {
                TempData["error"] = "Cannot delete the movie because associated showtimes exist.";
                return View(vm);
            }

            await movieRepo.Remove(movie);
            TempData["success"] = "Movie removed successfully!";
            return RedirectToAction("MovieList");
        }
    }
}

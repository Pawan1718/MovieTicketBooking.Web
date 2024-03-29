﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using MovieTicketBooking.Entities.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Web.ViewModels.TheaterViewModels;

namespace MovieTicketBooking.Web.Controllers
{
    public class TheaterController : Controller
    {
        private readonly ITheaterRepo theaterRepo;

        public TheaterController(ITheaterRepo _theaterRepo)
        {
            theaterRepo = _theaterRepo;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TheaterList(string filterText,int pageNo = 1, int pageSize = 3, string SearchText =null)
        {
            List<TheaterViewModel> theaterList = new List<TheaterViewModel>();
            var theaters =await theaterRepo.GetAll();
            if (SearchText!=null)
            {
                pageNo = 1;
            }
            else
            {
                SearchText = filterText;
               
            }
            ViewData["filterText"] = SearchText;

            int totalItems = 0;
            if (!string.IsNullOrEmpty(SearchText))
            {
                theaters=theaters.Where(x=>x.TheaterName.Contains(SearchText)).ToList();
            }
             totalItems= theaters.ToList().Count();
             theaters=theaters.Skip((pageNo - 1)* pageSize).Take(pageSize).ToList();
            foreach (var theater in theaters)
            {
                var vm = new TheaterViewModel
                {
                    Id= theater.Id,
                    TheaterName = theater.TheaterName,
                    Location = theater.Location,
                    Capacity = theater.Capacity,
                };
                theaterList.Add(vm);
            }
            var pvm = new PagedTheaterViewModel
            {
                Theaters = theaterList,
                PageInfo = new Utility.PageInfo
                {
                    PageNo=pageNo,
                    PageSize=pageSize,
                    TotalItems=totalItems
                }

            };
            return View(pvm);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTheater()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTheater(AddTheaterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (await theaterRepo.isTheaterExist(vm.TheaterName))
                {
                    TempData["error"] = "Theater already exists.";
                    return View(vm);
                }
                var theater = new Theater
                {
                    TheaterName = vm.TheaterName,
                    Location = vm.Location,
                    Capacity = vm.Capacity,
                };
                await theaterRepo.Save(theater);
                TempData["success"] = "Theater added successfully.";
                return View(vm);
            }
            TempData["error"] = "Failed to add theater. Please check the input data.";
            return View(vm);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditTheater(int id)
        {
            var theater = await theaterRepo.GetById(id);

            if (theater == null)
            {
                return NotFound();
            }

            TheaterViewModel vm = new TheaterViewModel
            {
                Id = theater.Id,
                TheaterName = theater.TheaterName,
                Location = theater.Location,
                Capacity = theater.Capacity,
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditTheater(TheaterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var theater = new Theater
                {
                    Id = vm.Id,
                    TheaterName = vm.TheaterName,
                    Location = vm.Location,
                    Capacity = vm.Capacity,
                };
                await theaterRepo.Edit(theater);
                TempData["success"] = "Theater updated successfully.";
                return RedirectToAction("TheaterList");
            }
            TempData["error"] = "Failed to update theater. Please check the input data.";
            return View(vm);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveTheater(int id)
        {
            if (id ==null)
            {
                return NotFound();
            }
         
            var theater = await theaterRepo.GetById(id);
            TheaterViewModel vm = new TheaterViewModel
            {
                Id = theater.Id,
                TheaterName = theater.TheaterName,
                Location = theater.Location,
                Capacity = theater.Capacity,
            };
            bool hasAssociatedData = await theaterRepo.isMovieExist(vm.Id) || await theaterRepo.isTheaterWithShowtimesExist(vm.Id);
            if (hasAssociatedData)
            {
                TempData["warning"] = "Associated movies or showtimes exist. Remove associated data before deleting the theater.";
            }
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveTheater(TheaterViewModel vm)
        {
            var theater = new Theater
            {
                Id = vm.Id,
                TheaterName = vm.TheaterName,
                Location = vm.Location,
                Capacity = vm.Capacity,
            };
            bool hasAssociatedData = await theaterRepo.isMovieExist(vm.Id) || await theaterRepo.isTheaterWithShowtimesExist(vm.Id);
            if (hasAssociatedData)
            {
                TempData["error"] = "Associated movies or showtimes exist. Remove associated data before deleting the theater.";
                return View(vm);
            }
            await theaterRepo.Remove(theater);
            TempData["success"] = "Theater removed successfully!";
            return RedirectToAction("TheaterList");
        }
    }
}

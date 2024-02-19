using Microsoft.AspNetCore.Identity;
using MovieTicketBooking.Entities.Models;
using MovieTicketBooking.Infrastructures;
using MovieTicketBooking.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBooking.Repositories.Implimentations
{
    public class DbInitial : IDbInitial
    {
        private UserManager <ApplicationUser> _userManager;
        private RoleManager <IdentityRole> _roleManager;

        public DbInitial(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task Seed()
        {
            if (!_roleManager.RoleExistsAsync(GlobalConfiguration.Admin_Role).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(GlobalConfiguration.Admin_Role)).GetAwaiter().GetResult();
                var user = new ApplicationUser
                {
                    Name = "Admin",
                    Mobile = 88888888,
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com",
                };
                _userManager.CreateAsync(user, "Admin@12345").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user, GlobalConfiguration.Admin_Role).GetAwaiter().GetResult();
            }
            return Task.CompletedTask;
         }
    }
}

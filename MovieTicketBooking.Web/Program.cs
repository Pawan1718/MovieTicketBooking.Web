using MovieTicketBooking.Repositories.Implimentations;
using MovieTicketBooking.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Entities.Models;
using MovieTicketBooking.Entities.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("MovieTicketBookingWebContextConnection") ?? throw new InvalidOperationException("Connection string 'MovieTicketBookingWebContextConnection' not found.");

//builder.Services.AddDbContext<MovieTicketBookingWebContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("cons"),b => b.MigrationsAssembly("MovieTicketBooking.Web")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(/*options => options.SignIn.RequireConfirmedAccount = true*/)
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMovieRepo, MovieRepo>();
builder.Services.AddScoped<ITheaterRepo, TheaterRepo>();
builder.Services.AddScoped<IBookingRepo, BookingRepo>();
builder.Services.AddScoped<IShowtimeRepo, ShowtimeRepo>();
builder.Services.AddScoped<IDbInitial, DbInitial>();
builder.Services.AddScoped<IUtilityRepo, UtilityRepo>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ITicketRepo, TicketRepo>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
DataSeeding();
void DataSeeding()
{
    using(var scope = app.Services.CreateScope())
    {
        var dbRepo = scope.ServiceProvider.GetRequiredService<IDbInitial>();
        dbRepo.Seed();
    }
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

namespace MovieTicketBooking.Web.ViewModels.MovieViewModels
{
    public class AddMovieViewModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public string? Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}

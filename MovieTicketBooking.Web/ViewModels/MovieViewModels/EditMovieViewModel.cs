namespace MovieTicketBooking.Web.ViewModels.MovieViewModels
{
    public class EditMovieViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public string? Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ChooseImage { get; set; }
    }
}

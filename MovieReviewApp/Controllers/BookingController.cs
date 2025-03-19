using Microsoft.AspNetCore.Mvc;
using MovieReviewApp.Data;
using MovieReviewApp.IRepository;
using MovieReviewApp.Models;
using MovieReviewApp.Services;

namespace MovieReviewApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly apiService _apiService;
        private readonly IRepository<BookingTicket> _bookingTicketRepository;
        private readonly IRepository<Movie> _movieRepository;
        public BookingController( apiService apiService, IRepository<BookingTicket> bookingTicketRepository, IRepository<Movie> movieRepository)
        {
            _apiService = apiService;
            _bookingTicketRepository = bookingTicketRepository;
            _movieRepository = movieRepository;
        }

        public async Task<IActionResult> Create(int Id)
        {
            // ✅ Check if there was an unbooked movie from a previous session
            if (TempData["MovieId"] != null)
            {
                int oldMovieId = (int)TempData["MovieId"];

                // ✅ Delete the old movie since no booking was made
                var oldMovie = await _movieRepository.GetById(oldMovieId);
                if (oldMovie != null)
                {
                    await _movieRepository.DeleteById(oldMovieId);
                }

                TempData.Remove("MovieId"); // ✅ Clear tracking
            }

            var MovieURL = $"https://api.themoviedb.org/3/movie/{Id}?api_key=039f54da1f3f70338722c1b60864daaf";

            Movie movieDatas = await _apiService.MovieApi<Movie>(MovieURL);
            if(movieDatas == null)
            {
                return NotFound("Movie not found");
            }

            await _movieRepository.Add(movieDatas);
            TempData["MovieId"] = movieDatas.DatabaseId;  // ✅ Track new movie

            var booking = new BookingTicket
            {
                movieData = movieDatas,
            };

            return View(booking);

        }

        [HttpPost]
        public async Task<IActionResult> SelectSeats(BookingTicket booking, int movieId)
        {

            var MovieURL = $"https://api.themoviedb.org/3/movie/{movieId}?api_key=039f54da1f3f70338722c1b60864daaf";

            Movie movieDatas = await _apiService.MovieApi<Movie>(MovieURL);
            if (movieDatas == null)
            {
                return NotFound("Movie not found");
            }


            var bookingData = new BookingTicket
            {
                Name = booking.Name,
                Id = booking.Id,
                MovieId = movieDatas.Id,
                movieData = movieDatas,
                Price = booking.Price,
                PhoneNumber = booking.PhoneNumber,
                BookingDate = booking.BookingDate,
                NumberOfSeats = booking.NumberOfSeats,
                SelectedSeats = booking.SelectedSeats,

            };

            await _bookingTicketRepository.Add(booking);
            return View(bookingData); // Pass booking data to seat selection page
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmBooking([FromBody] BookingTicket booking)
        {
            if (booking == null || string.IsNullOrEmpty(booking.SelectedSeats))
            {
                return BadRequest("Invalid booking data");
            }
             await _bookingTicketRepository.Add(booking);
            // Save booking to the database (Uncomment when database is connected)
            //_context.BookingTickets.Add(booking);
            //_context.SaveChanges();

            return Json(new { redirectUrl = Url.Action("Index", "Home") });

        }

    }

}

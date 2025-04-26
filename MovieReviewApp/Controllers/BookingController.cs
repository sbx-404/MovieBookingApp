    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewApp.Data;
    using MovieReviewApp.IRepository;
    using MovieReviewApp.Models;
    using MovieReviewApp.Services;
using Razorpay.Api;

    namespace MovieReviewApp.Controllers
    {
    public class BookingController : Controller
    {
        private readonly string _razorpayKey;
        private readonly string _razorpaySecret;
        private readonly string _apiKey;
        private readonly apiService _apiService;
        private readonly IRepository<BookingTicket> _bookingTicketRepository;
        private readonly IRepository<Movie> _movieRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        public BookingController(ApplicationDbContext applicationDbContext, IConfiguration configuration, apiService apiService, IRepository<BookingTicket> bookingTicketRepository, IRepository<Movie> movieRepository)
        {
            _razorpayKey = configuration["Razorpay:Key"];
            _razorpaySecret = configuration["Razorpay:Secret"];
            _apiKey = configuration["API_KEY:Key"];
            _apiService = apiService;
            _bookingTicketRepository = bookingTicketRepository;
            _movieRepository = movieRepository;
            _applicationDbContext = applicationDbContext;
        }


        public async Task<IActionResult> GetReservedSeats(int movieId)  
        {
            var seats = await _applicationDbContext.bookingTickets
                .Where(b => b.MovieId == movieId)
                .Select(b => b.SelectedSeats)
                .ToListAsync();

            return Json(seats);
        }



        public async Task<IActionResult> Create(int Id)
        {
            var MovieURL = $"https://api.themoviedb.org/3/movie/{Id}?api_key={_apiKey}";

            Movie movieDatas = await _apiService.MovieApi<Movie>(MovieURL);
            if (movieDatas == null)
            {
                return NotFound("Movie not found");
            }


            var existingMovie = await _movieRepository.GetAll();     // get all movies
            var movieInDb = existingMovie.FirstOrDefault(m => m.Ids == movieDatas.Ids);    // check the movie based on Ids is it present in database or not. 

            if (movieInDb == null)
            {
                await _movieRepository.Add(movieDatas); //  Save movie before assigning MovieId
                movieInDb = movieDatas; // point to the freshly saved movie
            }

            var booking = new BookingTicket
            {
                movieData = movieInDb,
                MovieId = movieInDb.DatabaseId // Correctly link movie
            };

            return View(booking);

        }

       

        [HttpPost]
        public IActionResult CreateRazorpayOrder(BookingTicket booking)
        {
            if (booking.PhoneNumber == null && string.IsNullOrEmpty(booking.SelectedSeats))
                return BadRequest("Invalid booking data!");

            RazorpayClient client = new RazorpayClient(_razorpayKey, _razorpaySecret);

            Dictionary<string, object> options = new Dictionary<string, object>
            {
                { "amount", booking.Price * 100 }, // Razorpay wants amount in paise
                { "currency", "INR" },
                { "receipt", "rcpt_" + Guid.NewGuid().ToString().Substring(0, 8) },
                { "payment_capture", 1 }
            };

            Razorpay.Api.Order order = client.Order.Create(options);

            return Json(new
            {
                orderId = order["id"].ToString(),
                key = _razorpayKey,
                amount = booking.Price * 100,
                name = booking.Name,
                phone = booking.PhoneNumber,
                seats = booking.SelectedSeats
            });      // this will going to frontend means goes to the javascript use in create.cshtml post
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(BookingTicket booking, string RazorpayPaymentId, string RazorpayOrderId)
        {
            if (string.IsNullOrEmpty(RazorpayPaymentId) )
            {
                return BadRequest("Payment was not successful or data is incomplete.");
            }
                       
            booking.RazorpayPaymentId = RazorpayPaymentId;
            booking.RazorpayOrderId = RazorpayOrderId;
            await _bookingTicketRepository.Add(booking);

            return RedirectToAction("BookingSuccess");
        }


        public IActionResult BookingSuccess()
        {
            return View();  // Create a simple ThankYou.cshtml for success message.
        }


    }
}

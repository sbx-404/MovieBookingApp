    using Microsoft.AspNetCore.Mvc;
    using MovieReviewApp.Data;
    using MovieReviewApp.IRepository;
    using MovieReviewApp.Models;
    using MovieReviewApp.Services;
using Razorpay.Api;

    namespace MovieReviewApp.Controllers
    {
    public class BookingController : Controller
    {
        private readonly apiService _apiService;
        private readonly IRepository<BookingTicket> _bookingTicketRepository;
        private readonly IRepository<Movie> _movieRepository;
        public BookingController(apiService apiService, IRepository<BookingTicket> bookingTicketRepository, IRepository<Movie> movieRepository)
        {
            _apiService = apiService;
            _bookingTicketRepository = bookingTicketRepository;
            _movieRepository = movieRepository;
        }

        public async Task<IActionResult> Create(int Id)
        {
            var MovieURL = $"https://api.themoviedb.org/3/movie/{Id}?api_key=039f54da1f3f70338722c1b60864daaf";

            Movie movieDatas = await _apiService.MovieApi<Movie>(MovieURL);
            if (movieDatas == null)
            {
                return NotFound("Movie not found");
            }


            var existingMovie = await _movieRepository.GetAll();
            var movieInDb = existingMovie.FirstOrDefault(m => m.Ids == movieDatas.Ids);

            //if (movieInDb == null)
            //{
            //    await _movieRepository.Add(movieDatas); // ✅ Save only if not exists
            //}

            if (movieInDb == null)
            {
                await _movieRepository.Add(movieDatas); // ✅ Save movie before assigning MovieId
                movieInDb = movieDatas; // point to the freshly saved movie
            }

            var booking = new BookingTicket
            {
                movieData = movieInDb,
                MovieId = movieInDb.DatabaseId // ✅ Correctly link movie
            };

            return View(booking);

        }

       

        [HttpPost]
        public IActionResult CreateRazorpayOrder(BookingTicket booking)
        {
            if (booking.PhoneNumber == null && string.IsNullOrEmpty(booking.SelectedSeats))
                return BadRequest("Invalid booking data!");

            RazorpayClient client = new RazorpayClient("rzp_test_PVyIf5WIssd0HH", "b8MDP1eI2i6SVShGcwKVPCh8");

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
                key = "rzp_test_PVyIf5WIssd0HH",
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

            return RedirectToAction("ThankYou");
        }


        public IActionResult BookingSuccess()
        {
            return View();  // Create a simple ThankYou.cshtml for success message.
        }


    }
}

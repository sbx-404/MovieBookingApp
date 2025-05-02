using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApp.Models;
using MovieReviewApp.Services;

namespace MovieReviewApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _apiKey;
        private readonly apiService _apiService;

        public HomeController(ILogger<HomeController> logger,IConfiguration configuration, apiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
            _apiKey = configuration["API_KEY:Key"];
        }

        public async Task<IActionResult>  Index()
        {
            var MovieURL = $"https://api.themoviedb.org/3/movie/upcoming?api_key={_apiKey}&language=en-US&page=1";
            MovieResponse MovieData = await _apiService.MovieApi<MovieResponse>(MovieURL);

            if (MovieData.Results == null)
            {
                return NotFound("Movie not found");
            }

            return View(MovieData.Results ?? new List<Movie>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

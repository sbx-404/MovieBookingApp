using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MovieReviewApp.Models;
using MovieReviewApp.Services;
using MovieReviewApp.Utility;
using Newtonsoft.Json.Linq;
using System.Security.Policy;


namespace MovieReviewApp.Controllers
{
    public class MovieController : Controller
    {
        private readonly apiService _apiService;
        private readonly string _apiKey;

        public MovieController(apiService apiService,IConfiguration configuration)
        {
            _apiService = apiService;
            _apiKey = configuration["API_KEY:Key"];
        }


        //[AllowAnonymous]
        public async Task<IActionResult> SearchMovie(string? query, int page = 1)
        {
            int defaultTotalPages = 23;    // total number of pages

            if (string.IsNullOrEmpty(query))
            {
                ViewBag.NotFoundMessage = $"Please enter a search term {query}";
                return View(new Pagination<Movie>(new List<Movie>(), 0, page, 1));    // prevent from the null exception error that's why give empty list of Movie 
            }

            var MovieUrl = $"https://api.themoviedb.org/3/search/movie?api_key={_apiKey}&query={query}&page={page}";

            // make api call after decenteralised in Movie model
            MovieResponse movieData = await _apiService.MovieApi<MovieResponse>(MovieUrl);
                        
            int movieItems = movieData.Results.Count();    //total number of movies display on the page

            if (movieData?.Results.Count == 0)
            {
                ViewBag.NotFoundMessage = $"No movies found for '{query}'";
                return View(new Pagination<Movie>(new List<Movie>(), 0, page, 1));           // prevent from the null exception error that's why give empty list of Movie 
            }

            int totalMoviePages = movieData.Total_pages < defaultTotalPages ? movieData.Total_pages : defaultTotalPages;

            var PaginateData = new Pagination<Movie>(movieData.Results , movieItems, page, totalMoviePages);
            ViewBag.SearchQuery = query;    // store the search query in viewbag. It remains after page reload.
            return View(PaginateData);
        }

        [Authorize(Roles = Roles.Role_Admin)]
        public async Task<IActionResult> PopularMovies(int page = 1)
        {
            int pageSize = 23;
            var url = $"https://api.themoviedb.org/3/movie/popular?api_key={_apiKey}&page={page}";

            MovieResponse data = await _apiService.MovieApi<MovieResponse>(url);

            if (data?.Results == null || !data.Results.Any())
            {
                ViewBag.NotFoundMessage = "No movies found";
                return View(data?.Results);
            }
            //return View(data?.Results ?? new List<Movie>());                     // if left side null then right will return

            int TotalMovies = data.Results.Count;
            
            // skip n number of items show next same n number of items.
            //var Movies = data.Results.Skip((page - 1) * pageSize).Take(pageSize).ToList();       // get number of items to display but this API already did the skip() and take() 

            var Pagination = new Pagination<Movie>(data.Results, TotalMovies, page, pageSize);

            return View(Pagination);
        }



        [Authorize(Roles = Roles.Role_User)]
        public async Task<IActionResult> TopRated()
        {
            var url = "https://api.themoviedb.org/3/movie/top_rated?api_key={_apiKey}";
            MovieResponse data = await _apiService.MovieApi<MovieResponse>(url); 
            return View(data?.Results ?? new List<Movie>());                     // if left side null then right will return
        }


        public async Task<IActionResult> MovieTrailer(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Movieurl = $"https://api.themoviedb.org/3/movie/{id}?api_key={_apiKey}";

            Movie MovieData = await _apiService.MovieApi<Movie>(Movieurl);

            var videoUrl = "https://api.themoviedb.org/3/movie/"+ id + "/videos?api_key={_apiKey}";
            MovieVideoResponse VideoData = await _apiService.MovieApi<MovieVideoResponse>(videoUrl);
    
            var trailerKey = VideoData?.Results?.FirstOrDefault(u => u.site == "YouTube" && u.type == "Trailer")?.Key;

            MovieTrailerViewModel viewModel = new MovieTrailerViewModel
            {
                movie = MovieData,
                VideoKey = trailerKey
            };

            return View(viewModel);
        }

    }   
}

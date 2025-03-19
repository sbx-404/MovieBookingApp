using Newtonsoft.Json;

namespace MovieReviewApp.Services
{
    public class apiService
    {

    private readonly HttpClient _client;

        public apiService(HttpClient httpClient)           // after the constructor is called , dependency injection is done in Program.cs the HttpClient will be created
        {
            _client = httpClient;
        }

        public async Task<T?> MovieApi<T>(string url)
        {
            var movie = await _client.GetAsync(url);   // get the movie data from the API
              
            if (movie.IsSuccessStatusCode)                  // check if the response is successful
            {      
                var json = await movie.Content.ReadAsStringAsync();  //convert the json data to string json data

                return JsonConvert.DeserializeObject<T>(json);  // Deserialize JSON into C# MovieResponse model  // means the json data is converted into c# model
            }
            else
            {
                return default; 
            }


        }

        }
    }

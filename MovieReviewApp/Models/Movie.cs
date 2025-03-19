using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MovieReviewApp.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DatabaseId { get; set; }


        [JsonProperty("Id")]
        public int Ids { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Poster_path { get; set; }

        public decimal Vote_average { get; set; }
        public string Release_date { get; set; }

        public int Runtime {  get; set; }

        [NotMapped]
        public List<Genre> Genres { get; set; }

        // Navigation property for Bookings
        public virtual ICollection<BookingTicket> BookingTickets { get; set; } = new List<BookingTicket>();
    }
    public class MovieResponse
    {
        public List<Movie> Results { get; set; }
        public int Total_pages { get; set; }
    }
}

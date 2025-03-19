namespace MovieReviewApp.Models
{
    public class MovieVideos
    {
        //public int Id { get; set; }
        public string site { get; set; }
        public string type { get; set; }

        public string Key { get; set; }
    }

    public class MovieVideoResponse
    {
        public List<MovieVideos> Results { get; set; }

    }
}

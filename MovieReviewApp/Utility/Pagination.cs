using MovieReviewApp.Models;

namespace MovieReviewApp.Utility
{
    public class Pagination<T>
    {

        public List<T> Movies { get; set; }
        public int PageItems { get; set; }  
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public Pagination(List<T> movie, int pageItems, int currentPage, int totalPages)
        {
            Movies = movie;
            PageItems = pageItems;
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }

        public bool IsPreviousPageAvailable => CurrentPage > 1;
        public bool IsNextPageAvailable => CurrentPage < TotalPages;

    }
}

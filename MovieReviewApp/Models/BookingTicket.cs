using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReviewApp.Models
{
    public class BookingTicket
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Movie))]
        public int MovieId { get; set; }

        // Navigation property for EF Core
        public virtual Movie movieData { get; set; }

        [Required]
        public string Name { get; set; } = "Guest";

        [Required]
        public decimal Price { get; set; } = 0;

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; } = "0000000000";

        [Required]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        [Required]
        public int NumberOfSeats { get; set; }

        [Required]
        public string SelectedSeats { get; set; } = "";
    }
}

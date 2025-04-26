using Microsoft.AspNetCore.Identity;
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

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } 

        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; } 

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; } 

        [Required(ErrorMessage = "Booking date is required.")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; } 

        
        [Required(ErrorMessage = "Number of seats is required.")]
        public int NumberOfSeats { get; set; }


        [Required(ErrorMessage = "Please select at least one seat.")]
        public string SelectedSeats { get; set; } 


        public string RazorpayPaymentId { get; set; }
        public string RazorpayOrderId { get; set; }
        //public string RazorpaySignature { get; set; }
        //public string PaymentStatus { get; set; }
       

    }
}

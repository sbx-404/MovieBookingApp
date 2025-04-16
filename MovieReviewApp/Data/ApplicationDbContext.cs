using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieReviewApp.Models;
using MovieReviewApp.Models.AuthModel;

namespace MovieReviewApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<MovieReviewAuthModel> MovieReviewAuthModel { get; set; }
        public DbSet<BookingTicket> bookingTickets { get; set; }
        public DbSet<Movie> Movies { get; set; }
        //public DbSet<Genre> Genres { get; set; }
        //public DbSet<LoginModel> Login { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<BookingTicket>()
        //        .HasOne(bt => bt.movieData)
        //        .WithMany(m => m.BookingTickets)
        //        .HasForeignKey(bt => bt.MovieId) // ✅ Ensure it references Movie.DatabaseId
        //        .OnDelete(DeleteBehavior.Cascade);
        //}
    }

}

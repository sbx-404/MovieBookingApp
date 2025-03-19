using Microsoft.AspNetCore.Identity;

namespace MovieReviewApp.Models.AuthModel
{
    public class MovieReviewAuthModel : IdentityUser
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime CreatedBy { get; set; } = DateTime.Now;
        public DateTime UpdatedBy { get; set; } = DateTime.Now;
    }
}

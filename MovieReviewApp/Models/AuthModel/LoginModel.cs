using Microsoft.AspNetCore.Identity;

namespace MovieReviewApp.Models.AuthModel
{
    public class LoginModel 
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}

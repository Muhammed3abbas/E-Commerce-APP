using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Api.Dtos
{
    public class RegisterDto
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, one non-alphanumeric character, and be at least 8 characters long.")]

        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}

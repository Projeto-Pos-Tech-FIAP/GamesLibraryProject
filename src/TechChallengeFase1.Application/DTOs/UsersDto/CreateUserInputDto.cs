using System.ComponentModel.DataAnnotations;
using TechChallengeFase1.Domain.Enums;

namespace TechChallengeFase1.Application.DTOs.UsersDto
{
    public class CreateUserInputDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        public LevelEnum Level { get; set; } = LevelEnum.User;

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        public GenderEnum Gender { get; set; } = GenderEnum.Other;
    }
}
using TechChallengeFase1.Domain.Enums;

namespace TechChallengeFase1.Application.DTOs.UsersDto
{
    public class EditUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public GenderEnum? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}

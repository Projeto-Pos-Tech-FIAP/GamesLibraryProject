using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechChallengeFase1.Domain.DTOs
{
    public class UserRegistrationInputDto
    {
        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O E-mail deve ser um endereço de e-mail válido.")]
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string DocumentNumber { get; set; }
    }
}

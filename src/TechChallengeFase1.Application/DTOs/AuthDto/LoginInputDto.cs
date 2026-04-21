using System.ComponentModel.DataAnnotations;

public class LoginInputDto
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
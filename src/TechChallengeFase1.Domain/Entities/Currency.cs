using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechChallengeFase1.Domain.Entities;

[Table("Currency")]
public class Currency : SoftDelete
{
    [Key]
    public int CurrencyId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Description { get; set; } = null!;

    public Currency(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public void Update(string code, string description)
    {
        Code = code;
        Description = description;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechChallengeFase1.Domain.Entities;

[Table("OrderStatus")]
public class OrderStatus : SoftDelete
{
    [Key]
    public int OrderStatusId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Description { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public OrderStatus(string description)
    {
        Description = description;
    }

    public void Update(string description)
    {
        Description = description;
    }
}

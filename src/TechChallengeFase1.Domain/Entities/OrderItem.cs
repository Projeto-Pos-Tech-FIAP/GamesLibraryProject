using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechChallengeFase1.Domain.Entities;

[Table("OrderItem")]
public class OrderItem : SoftDelete
{
    [Key]
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int GameId { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public Order? Order { get; set; }

    public Game? Game { get; set; }

    public OrderItem(int orderId, int gameId, decimal price)
    {
        OrderId = orderId;
        GameId = gameId;
        Price = price;
    }

    public void Update(int orderId, int gameId, decimal price)
    {
        OrderId = orderId;
        GameId = gameId;
        Price = price;
    }
}

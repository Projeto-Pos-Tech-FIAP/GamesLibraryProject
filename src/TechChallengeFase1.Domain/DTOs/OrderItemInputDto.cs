namespace TechChallengeFase1.Domain.DTOs;

public class OrderItemInputDto
{
    public int OrderId { get; set; }
    public int GameId { get; set; }
    public decimal Price { get; set; }
}

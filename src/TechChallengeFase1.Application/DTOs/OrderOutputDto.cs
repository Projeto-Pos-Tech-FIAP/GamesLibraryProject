using TechChallengeFase1.Domain.Enums;

namespace TechChallengeFase1.Application.DTOs;

public class OrderOutputDto
{
    public int OrderId { get; set; }
    public Guid UserGuid { get; set; }
    public decimal TotalPrice { get; set; }
    public int CurrencyId { get; set; }
    public CurrencyOutputDto? Currency { get; set; }
    public int OrderStatusId { get; set; }
    public OrderStatusEnum OrderStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<OrderItemOutputDto> OrderItems { get; set; } = new List<OrderItemOutputDto>();
}

public class OrderItemOutputDto
{
    public int OrderItemId { get; set; }
    public int GameId { get; set; }
    public string GameTitle { get; set; } = null!;
    public decimal Price { get; set; }
}

public class CurrencyOutputDto
{
    public int CurrencyId { get; set; }
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
}

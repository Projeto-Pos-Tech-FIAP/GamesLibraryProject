using System;

namespace TechChallengeFase1.Domain.DTOs;

public class OrderInputDto
{
    public Guid UserGuid { get; set; }
    public decimal TotalPrice { get; set; }
    public int CurrencyId { get; set; }
    public int OrderStatusId { get; set; }
    public Guid CreatedBy { get; set; }
}

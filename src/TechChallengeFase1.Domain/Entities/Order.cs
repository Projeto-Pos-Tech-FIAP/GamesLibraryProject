using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechChallengeFase1.Domain.Entities;

[Table("Order")]
public class Order : SoftDelete
{
    [Key]
    public int OrderId { get; set; }

    public Guid UserGuid { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    public int CurrencyId { get; set; }

    public int OrderStatusId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? PaidAt { get; set; }

    public OrderStatus? OrderStatus { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public Order(Guid userGuid, decimal totalPrice, int currencyId, int orderStatusId, Guid createdBy)
    {
        UserGuid = userGuid;
        TotalPrice = totalPrice;
        CurrencyId = currencyId;
        OrderStatusId = orderStatusId;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void Update(decimal totalPrice, int currencyId, int orderStatusId, Guid updatedBy, DateTime? paidAt = null)
    {
        TotalPrice = totalPrice;
        CurrencyId = currencyId;
        OrderStatusId = orderStatusId;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        PaidAt = paidAt;
    }
}

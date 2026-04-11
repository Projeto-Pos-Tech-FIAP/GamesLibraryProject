using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.Enums;

namespace TechChallengeFase1.Application.Interfaces;

public interface IOrderService
{
    Task<OrderOutputDto> CreateOrderAsync(OrderInputDto dto);
    Task<OrderOutputDto> ChangeStatusAsync(int orderId, OrderStatusEnum newStatus, Guid updatedBy);
    Task<OrderOutputDto?> GetByIdAsync(int orderId);
}

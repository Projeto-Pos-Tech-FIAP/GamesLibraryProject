using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order);
    Task<Order?> GetByIdAsync(int orderId);
    Task UpdateAsync(Order order);
}

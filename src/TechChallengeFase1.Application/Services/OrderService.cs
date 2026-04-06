using AutoMapper;
using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.Entities;
using TechChallengeFase1.Domain.Enums;
using TechChallengeFase1.Domain.Interfaces;

namespace TechChallengeFase1.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<OrderOutputDto> CreateOrderAsync(OrderInputDto dto)
    {
        var order = new Order(
            dto.UserGuid,
            dto.TotalPrice,
            dto.CurrencyId,
            dto.OrderStatusId,
            dto.CreatedBy
        );

        var createdOrder = await _orderRepository.AddAsync(order);
        return _mapper.Map<OrderOutputDto>(createdOrder);
    }

    public async Task<OrderOutputDto> ChangeStatusAsync(int orderId, OrderStatusEnum newStatus, Guid updatedBy)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            throw new Exception("Pedido não encontrado.");
        }

        order.Update(
            order.TotalPrice,
            order.CurrencyId,
            (int)newStatus,
            updatedBy,
            newStatus == OrderStatusEnum.Pago ? DateTime.UtcNow : order.PaidAt
        );

        await _orderRepository.UpdateAsync(order);
        return _mapper.Map<OrderOutputDto>(order);
    }

    public async Task<OrderOutputDto?> GetByIdAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        return _mapper.Map<OrderOutputDto>(order);
    }
}

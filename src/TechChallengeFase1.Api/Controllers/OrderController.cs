using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.Enums;

namespace TechChallengeFase1.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Cria uma nova ordem (venda).
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(OrderOutputDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrderOutputDto>> CreateAsync([FromBody] OrderInputDto dto)
    {
        var output = await _orderService.CreateOrderAsync(dto);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = output.OrderId }, output);
    }

    /// <summary>
    /// Altera o status de uma ordem.
    /// </summary>
    [HttpPatch("{id:int}/status-change")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(OrderOutputDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrderOutputDto>> ChangeStatusAsync(int id, [FromBody] OrderStatusEnum newStatus)
    {
        // Pega um Guid dummy para o updatedBy se não houver autenticação (conforme plano)
        var updatedBy = Guid.Empty; 
        var output = await _orderService.ChangeStatusAsync(id, newStatus, updatedBy);
        return Ok(output);
    }

    /// <summary>
    /// Busca uma ordem por ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OrderOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderOutputDto>> GetByIdAsync(int id)
    {
        var output = await _orderService.GetByIdAsync(id);
        if (output == null) return NotFound();
        return Ok(output);
    }
}

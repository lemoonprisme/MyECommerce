using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyECommerce.Api.Dtos;
using MyECommerce.Application.Commands;
using MyECommerce.Domain;
using MyECommerce.Infrastructure;

namespace MyECommerce.Api;

[Route("api/order")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        var request = new CreateOrder.Request(
            createOrderDto.Address,
            createOrderDto.Products.Select(s => new OrderItem()
            {
                ProductId = s.ProductId,
                Quantity = s.Quantity
            }).ToList(),
            Convert.ToInt64(User.FindFirstValue(ClaimTypes.NameIdentifier))
        );
        var order = await _mediator.Send(request);

        return Ok(order);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var request = new GetOrder.Request(id, User.IsInRole(RoleConsts.ViewAllOrders));
        var order = await _mediator.Send(request);
        if (order == null)
            return NotFound(); //if there is no access will return NotFound
        return Ok(order);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserOrders()
    {
        var request = new GetUserOrders.Request(Convert.ToInt64(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        var orders = await _mediator.Send(request);
        if (orders.IsNullOrEmpty())
            return NotFound();
        return Ok(orders);
    }

    [Authorize(Policy = "Admins")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        var request = new DeleteOrder.Request(id);
        var deletedRows = await _mediator.Send(request);

        if (deletedRows > 0)
            return NoContent();
        return NotFound();
    }
}
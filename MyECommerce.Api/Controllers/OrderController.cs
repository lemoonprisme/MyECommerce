using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public OrderController(ApplicationContext context, IMediator mediator)
    {
        _mediator = mediator;
    }

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
            createOrderDto.UserId
        );
        var order = await _mediator.Send(request);

        return Ok(order);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var request = new GetOrder.Request(id);
        var order = await _mediator.Send(request);
        if (order == null)
            return NotFound();
        return Ok(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var request = new DeleteOrder.Request(id);
        var deletedRows = await _mediator.Send(request);

        if (deletedRows > 0)
            return NoContent();
        return NotFound();
    }
}
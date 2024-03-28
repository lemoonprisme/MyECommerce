using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyECommerce.Api.Dtos;
using MyECommerce.Domain;
using MyECommerce.Infrastructure;

namespace MyECommerce.Api;


[Route("api/order")]
[ApiController]
public class OrderController:ControllerBase
{
    private readonly ApplicationContext _context;

    public OrderController(ApplicationContext context)
    {
        _context = context;
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        var order = new Order()
        {
            Id = Guid.NewGuid(),
            Address = createOrderDto.Address,
            Products = createOrderDto.Products
        };
        _context.Add(order);
        await _context.SaveChangesAsync();
        return Ok(order);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _context.FindAsync<Order>(id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var deletedRows = await _context.Orders.Where(s => s.Id == id).ExecuteDeleteAsync();
        if (deletedRows > 0)
            return NoContent();
        return NotFound();
    }
    
}
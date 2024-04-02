using MyECommerce.Domain;

namespace MyECommerce.Api.Dtos;

public class CreateOrderDto
{
    public Address Address { get; set; }
    public List<OrderItemDto> Products { get; set; }
}


public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
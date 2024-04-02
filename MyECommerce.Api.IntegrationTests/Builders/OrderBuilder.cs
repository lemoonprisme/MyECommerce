using MyECommerce.Api.Dtos;
using MyECommerce.Domain;

namespace MyECommerce.Api.IntegrationTests.Builders;

internal class OrderBuilder(long userId)
{
    private readonly Order _order = new()
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Address = new Address() { City = "string", State = "string", Street = "string", Zip = "string" },
        Products = new List<OrderItem>()
        {
            new OrderItem() { ProductId = Guid.NewGuid(), Quantity = 1 }
        }
    };

    public OrderBuilder SetId(Guid id)
    {
        _order.Id = id;
        return this;
    }

    public OrderBuilder SetAddress(Address address)
    {
        _order.Address = address;
        return this;
    }

    public OrderBuilder AddProducts(OrderItem orderItem)
    {
        _order.Products.Add(orderItem);
        return this;
    }

    public OrderBuilder CleanProducts()
    {
        _order.Products.Clear();
        return this;
    }

    public CreateOrderDto GetCreateOrderDto()
    {
        return new CreateOrderDto()
        {
            Address = _order.Address,
            Products = _order.Products.Select(s =>
            {
                return new OrderItemDto()
                {
                    ProductId = s.ProductId,
                    Quantity = s.Quantity
                };
            }).ToList()
        };
    }

    public static implicit operator Order(OrderBuilder orderBuilder)
    {
        return orderBuilder._order;
    }
}
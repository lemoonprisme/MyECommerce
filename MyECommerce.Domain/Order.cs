namespace MyECommerce.Domain;

public class Order
{
    public Guid Id { get; set; }

    public long UserId { get; set; }

    public ApplicationUser User { get; set; }
    public Address Address { get; set; }
    public List<OrderItem> Products { get; set; }
}

public class OrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
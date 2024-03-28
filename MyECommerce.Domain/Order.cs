namespace MyECommerce.Domain;

public class Order
{
    public Guid Id { get; set; }
    public Address Address { get; set; }
    public List<Guid> Products { get; set; }
}
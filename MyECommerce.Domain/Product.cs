namespace MyECommerce.Domain;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public Status Status { get; set; }
}
public enum Status
{
    Available,
    Sold
}
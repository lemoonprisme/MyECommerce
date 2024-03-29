using MyECommerce.Domain;

namespace MyECommerce.Api.Dtos;

public class PutProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public Status Status { get; set; }
}
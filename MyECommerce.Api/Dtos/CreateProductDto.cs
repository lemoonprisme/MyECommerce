using MyECommerce.Domain;

namespace MyECommerce.Api.Dtos;

public class CreateProductDto
{
    public string Name { get; set; }
    public string Category { get; set; }
    public Status Status { get; set; }

}



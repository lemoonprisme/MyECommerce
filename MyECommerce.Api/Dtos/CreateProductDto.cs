using FluentValidation;
using MyECommerce.Domain;

namespace MyECommerce.Api.Dtos;

public class CreateProductDto
{
    public string Name { get; set; }
    public string Category { get; set; }
    public Status Status { get; set; }
    
}

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(s => s.Name).Length(0, 50);
        RuleFor(s => s.Category).Length(0, 20);
    }
}


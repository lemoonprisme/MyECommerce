using FluentValidation;
using MyECommerce.Domain;

namespace MyECommerce.Api.Dtos;

public class CreateOrderDto
{
    public Address Address { get; set; }
    public List<Guid> Products { get; set; }
}
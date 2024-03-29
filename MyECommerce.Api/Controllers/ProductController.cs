using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyECommerce.Api.Dtos;
using MyECommerce.Application.Commands;
using MyECommerce.Domain;
using MyECommerce.Infrastructure;

namespace MyECommerce.Api.Controllers;

[Route("api/product")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;


    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Add a new product
    /// </summary>
    /// <param name="productDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(Product), 200)]
    [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), 400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductDto productDto,
        CancellationToken cancellationToken)
    {
        var request = new CreateProduct.Request(productDto.Name, productDto.Category, productDto.Status);
        var product = await _mediator.Send(request, cancellationToken);

        return Ok(product);
    }
    
    /// <summary>
    /// Get the product by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetProduct.Request(id);
        var product = await _mediator.Send(request, cancellationToken);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }
    
    /// <summary>
    /// Delete product with Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteProduct.Request(id);
        var deletedRows = await _mediator.Send(request, cancellationToken);
        if (deletedRows > 0)
            return NoContent();
        return NotFound();
    }
    
    /// <summary>
    /// Adds or updates the product
    /// </summary>
    /// <param name="productDto"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [ProducesResponseType(typeof(Product), 200)]
    [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails), 400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> PutProduct([FromBody] PutProductDto productDto)
    {
        var request = new PutProduct.Request(productDto.Id, productDto.Name, productDto.Category, productDto.Status);
        var product = await _mediator.Send(request);
        return Ok(product);
    }
}
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyECommerce.Api.Dtos;
using MyECommerce.Domain;
using MyECommerce.Infrastructure;

namespace MyECommerce.Api.Controllers;
[Route("api/product")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ApplicationContext _applicationContext;
    private readonly IValidator<CreateProductDto> _validator;
    private readonly ILogger<ProductController> _logger;

    public ProductController(ApplicationContext applicationContext, 
        IValidator<CreateProductDto> validator, 
        ILogger<ProductController> logger)
    {
        _applicationContext = applicationContext;
        _validator = validator;
        _logger = logger;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductDto productDto)
    {
        var validationResult = await _validator.ValidateAsync(productDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem();
        } 
        var product = new Product()
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            Category = productDto.Category,
            Status = productDto.Status
        };
        _applicationContext.Products.Add(product);
        await _applicationContext.SaveChangesAsync();
        
        return Ok(product);
    }
    
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
            var product = await _applicationContext.FindAsync<Product>(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var deletedRows = await _applicationContext.Products.Where(s => s.Id == id).ExecuteDeleteAsync();
        if (deletedRows > 0)
            return NoContent();
        return NotFound();

    }
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> PutProduct([FromBody] PutProductDto productDto)
    {
        var product = new Product()
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Category = productDto.Category,
            Status = productDto.Status
        };
        try
        {
            _applicationContext.Add(product);
            await _applicationContext.SaveChangesAsync();
            return Ok(product);
        }
        catch (DbUpdateException e)
        {
            foreach (var entry in e.Entries)
            {
                entry.State = EntityState.Modified;
            }
            await _applicationContext.SaveChangesAsync();
        }
        return Ok(product);
    }
}
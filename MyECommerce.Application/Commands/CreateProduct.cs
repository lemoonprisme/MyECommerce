using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using MyECommerce.Domain;
using MyECommerce.Infrastructure;

namespace MyECommerce.Application.Commands;

public static class CreateProduct
{
    public record Request(string Name, string Category, Status Status) : IRequest<Product>;
    
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(s => s.Name).Length(0, 50);
            RuleFor(s => s.Category).Length(0, 20);
        }
    }
    
    [UsedImplicitly]
    public class Handler : IRequestHandler<Request,Product>
    {
        private readonly ApplicationContext _applicationContext;

        public Handler(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        
        public async Task<Product> Handle(Request request, CancellationToken cancellationToken)
        {
            
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Category = request.Category,
                Status = request.Status
            };
            _applicationContext.Products.Add(product);
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return product;
        }
    }
}
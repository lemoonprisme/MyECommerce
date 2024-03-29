using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyECommerce.Domain;
using MyECommerce.Infrastructure;

namespace MyECommerce.Application.Commands;

public static class PutProduct
{
    public record Request(Guid Id, string Name, string Category, Status Status) : IRequest<Product>;

    [UsedImplicitly]
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(s => s.Name).Length(0, 50);
            RuleFor(s => s.Category).Length(0, 20);
        }
    }

    [UsedImplicitly]
    public class Handler : IRequestHandler<Request, Product>
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
                Id = request.Id,
                Name = request.Name,
                Category = request.Category,
                Status = request.Status
            };
            try
            {
                _applicationContext.Add(product);
                await _applicationContext.SaveChangesAsync(cancellationToken);
                return product;
            }
            catch (DbUpdateException e)
            {
                foreach (var entry in e.Entries)
                {
                    entry.State = EntityState.Modified;
                }
                await _applicationContext.SaveChangesAsync(cancellationToken);
            }
            return product;
        }
    }
}
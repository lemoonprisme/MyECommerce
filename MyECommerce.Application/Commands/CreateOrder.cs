using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using MyECommerce.Domain;
using MyECommerce.Infrastructure;

namespace MyECommerce.Application.Commands;

public static class CreateOrder
{
    public record Request(Address Address, List<OrderItem> Products, long UserId) : IRequest<Order>;

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(s => s.Address.City).NotEmpty();
            RuleFor(s => s.Address.State).NotEmpty();
            RuleFor(s => s.Address.Street).NotEmpty();
            RuleFor(s => s.Address.Zip).NotEmpty();
            RuleFor(s => s.Products.Count).LessThanOrEqualTo(10).GreaterThan(0);
            RuleForEach(s => s.Products).Must(s => s.Quantity > 0)
                .WithMessage("Product quantity should be more than 0");
        }
    }

    [UsedImplicitly]
    public class Handler : IRequestHandler<Request, Order>
    {
        private readonly ApplicationContext _applicationContext;

        public Handler(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<Order> Handle(Request request, CancellationToken cancellationToken)
        {
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                Address = request.Address,
                Products = request.Products,
                UserId = request.UserId,
            };
            _applicationContext.Add(order);
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return order;
        }
    }
}
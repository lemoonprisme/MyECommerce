using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using MyECommerce.Domain;
using MyECommerce.Domain.Events;
using MyECommerce.Infrastructure;

namespace MyECommerce.Application.Commands;

public static class CreateOrder
{
    public record Request(Address Address, List<OrderItem> Products, long UserId) : IRequest<Order>;
    
    [UsedImplicitly]
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
        private readonly IPublisher _publisher;

        public Handler(ApplicationContext applicationContext, IPublisher publisher)
        {
            _applicationContext = applicationContext;
            _publisher = publisher;
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
            await _publisher.Publish(new OrderCreatedEvent(order.UserId, order.Id, order.Address), cancellationToken);
            return order;
        }
    }
}
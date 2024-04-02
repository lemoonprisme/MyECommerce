using MediatR;
using Microsoft.EntityFrameworkCore;
using MyECommerce.Domain;
using MyECommerce.Infrastructure;

namespace MyECommerce.Application.Commands;

public static class GetUserOrders
{
    public record Request(long UserId) : IRequest<List<Order>>;

    public class Handler : IRequestHandler<Request, List<Order>>
    {
        private readonly ApplicationContext _applicationContext;

        public Handler(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public Task<List<Order>> Handle(
            Request request,
            CancellationToken cancellationToken)
        {
            return _applicationContext.Orders
                .Where(s => s.UserId == request.UserId)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
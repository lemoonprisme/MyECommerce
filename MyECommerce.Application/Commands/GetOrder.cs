using JetBrains.Annotations;
using MediatR;
using MyECommerce.Domain;
using MyECommerce.Infrastructure;

namespace MyECommerce.Application.Commands;
public static class GetOrder
{
    public record Request(Guid Id) : IRequest<Order?>;

    [UsedImplicitly]
    public class Handler : IRequestHandler<Request, Order?>
    {
        private readonly ApplicationContext _applicationContext;

        public Handler(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<Order?> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _applicationContext.FindAsync<Order>([request.Id], cancellationToken);
        }
    }
}
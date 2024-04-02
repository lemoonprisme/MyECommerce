using JetBrains.Annotations;
using MediatR;
using MyECommerce.Domain;
using MyECommerce.Infrastructure;

namespace MyECommerce.Application.Commands;

public static class GetProduct
{
    public record Request(Guid Id) : IRequest<Product?>;

    [UsedImplicitly]
    public class Handler : IRequestHandler<Request, Product?>
    {
        private readonly ApplicationContext _applicationContext;

        public Handler(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<Product?> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _applicationContext.FindAsync<Product>([request.Id], cancellationToken);
        }
    }
}
﻿using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyECommerce.Infrastructure;

namespace MyECommerce.Application.Commands;

public static class DeleteProduct
{
    public record Request(Guid Id) : IRequest<int>;

    [UsedImplicitly]
    public class Handler : IRequestHandler<Request, int>
    {
        private readonly ApplicationContext _applicationContext;

        public Handler(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            return await _applicationContext.Products.Where(s => s.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
        }
    }
}
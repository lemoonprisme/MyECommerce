using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MyECommerce.Infrastructure;

namespace MyECommerce.Api.Services;

public class MigrationService : IHostedService
{
    private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1,1);
    private readonly IServiceProvider _serviceProvider;

    public MigrationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        await SemaphoreSlim.WaitAsync(cancellationToken);
        try
        {
            await scope.ServiceProvider.GetRequiredService<ApplicationContext>()
                .GetInfrastructure().GetRequiredService<IMigrator>()
                .MigrateAsync(cancellationToken: cancellationToken);
        }
        finally
        {
            SemaphoreSlim.Release();
        }
        
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
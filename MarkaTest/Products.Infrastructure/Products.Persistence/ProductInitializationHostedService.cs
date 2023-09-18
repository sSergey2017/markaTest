using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Products.Application.Interfaces;

namespace Products.Persistence;

public class ProductInitializationHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ProductInitializationHostedService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var productInitializationService = scope.ServiceProvider.GetRequiredService<IProductInitializationService>();
            await productInitializationService.InitializeProduct();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

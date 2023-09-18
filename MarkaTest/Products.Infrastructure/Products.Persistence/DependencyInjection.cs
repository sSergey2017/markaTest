using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Products.Application.Interfaces;

namespace Products.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection
        services, IConfiguration configuration)
    {
        services.AddDbContext<ProductDbContext>(options =>
            options.UseInMemoryDatabase("ProductDataDb"));
        
        services.AddScoped<IProductDbContext, ProductDbContext>();
        services.AddScoped<IProductRepository, ProductRepository>();
        SD.MockyAPIBase = configuration["ServiceUrls:MockyAPI"];
        services.AddHttpClient<IProductInitializationService, ProductInitializationService>("MockyAPI", c =>
        {
            if (SD.MockyAPIBase != null) c.BaseAddress = new Uri(SD.MockyAPIBase);
        });
        services.AddScoped<IProductInitializationService, ProductInitializationService>();
        services.AddHostedService<ProductInitializationHostedService>();

        return services;
    }
}
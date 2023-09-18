﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Products.Application.Interfaces;

namespace Products.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection
        services)
    {
        services.AddDbContext<ProductDbContext>(options =>
            options.UseInMemoryDatabase("ProductDataDb"));
        
        services.AddScoped<IProductDbContext, ProductDbContext>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
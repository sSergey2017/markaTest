using Microsoft.EntityFrameworkCore;
using Products.Domain;

namespace Products.Application.Interfaces;

public interface IProductDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Size> ProductSizes { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
using Microsoft.EntityFrameworkCore;
using Products.Application.Interfaces;
using Products.Domain;
using Products.Persistence.EntityTypeConfigurations;

namespace Products.Persistence;

public class ProductDbContext : DbContext, IProductDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Size> ProductSizes { get; set; }

    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new SizeConfiguration());
        base.OnModelCreating(builder);
    }
}
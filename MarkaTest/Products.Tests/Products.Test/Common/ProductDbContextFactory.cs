using Microsoft.EntityFrameworkCore;
using Products.Domain;
using Products.Persistence;

namespace Products.Test.Common;

public class ProductDbContextFactory
{
    public static ProductDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new ProductDbContext(options);
        context.Database.EnsureCreated();
        
        context.Products.AddRange(
            new Product
            {
                Title = "A Red Trouser",
                Price = 10,
                Sizes = new List<Size>
                {
                    new Size { Name = "small" },
                    new Size { Name = "medium" },
                    new Size { Name = "large" }
                },
                Description = "This trouser perfectly pairs with a green shirt."
            },
            new Product
            {
                Title = "A Green Trouser",
                Price = 11,
                Sizes = new List<Size>
                {
                    new Size { Name = "small" }
                },
                Description = "This trouser perfectly pairs with a blue shirt."
            },
            new Product
            {
                Title = "A Blue Trouser",
                Price = 12,
                Sizes = new List<Size>
                {
                    new Size { Name = "medium" }
                },
                Description = "This trouser perfectly pairs with a red shirt."
            },
            new Product
            {
                Title = "A Red Trouser",
                Price = 13,
                Sizes = new List<Size>
                {
                    new Size { Name = "large" }
                },
                Description = "This trouser perfectly pairs with a green shirt."
            },
            new Product
            {
                Title = "A Green Trouser",
                Price = 14,
                Sizes = new List<Size>
                {
                    new Size { Name = "small" },
                    new Size { Name = "medium" }
                },
                Description = "This trouser perfectly pairs with a blue shirt."
            }
            );
        
        context.SaveChanges();
        return context;
    }
    
    public static void Destroy(ProductDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
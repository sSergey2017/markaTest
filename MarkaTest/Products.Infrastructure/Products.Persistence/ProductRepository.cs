using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Products.Application.Interfaces;
using Products.Application.Products.Queries.GetProductsFilter;
using Products.Domain;
using LinqKit;

namespace Products.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly IProductDbContext _context;

    public ProductRepository(IProductDbContext context)
    {
        _context = context;
        if (!_context.Products.Any())
        {
             AddData();
        }
    }

    private async Task AddData()
    {
        var cancellationToken = new CancellationToken();
        var products = new List<Product>
        {
            new Product 
            {
                Id = 1,
                Title = "Shirt",
                Price = 50,
                Description = "A white cotton shirt.",
                Sizes = new List<Size>
                {
                    new Size { Id = 1, Name = "S", ProductId = 1 },
                    new Size { Id = 2, Name = "M", ProductId = 1 },
                    new Size { Id = 3, Name = "L", ProductId = 1 },
                }
            },
            new Product 
            {
                Id = 2,
                Title = "Jeans",
                Price = 80,
                Description = "Blue denim jeans.",
                Sizes = new List<Size>
                {
                    new Size { Id = 4, Name = "M", ProductId = 2 },
                    new Size { Id = 5, Name = "L", ProductId = 2 },
                    new Size { Id = 6, Name = "XL", ProductId = 2 },
                }
            },
            new Product 
            {
                Id = 3,
                Title = "Sneakers",
                Price = 120,
                Description = "Comfortable running sneakers.",
                Sizes = new List<Size>
                {
                    new Size { Id = 7, Name = "8", ProductId = 3 },
                    new Size { Id = 8, Name = "9", ProductId = 3 },
                    new Size { Id = 9, Name = "10", ProductId = 3 },
                }
            }
        };
        await _context.Products.AddRangeAsync(products, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<Product>> GetAllProducts(CancellationToken cancellationToken)
    {
        return await _context.Products.ToListAsync(cancellationToken);
    }

    public async Task AddProduct(Product model, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(model, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
    }

    public async Task AddProducts(IList<Product> model, CancellationToken cancellationToken)
    {
        await _context.Products.AddRangeAsync(model, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<Product>> GetProductsByQuery(GetProductsFilterQuery query, CancellationToken cancellationToken)
    {
        var predicate = BuildPredicate(query);
        return await _context.Products.Where(predicate).ToListAsync(cancellationToken);
    }
    
    private static Expression<Func<Product, bool>> BuildPredicate(GetProductsFilterQuery query)
    {
        var predicate = PredicateBuilder.True<Product>();

        if (query.Price > 0)
        {
            predicate = predicate.And(p => p.Price <= query.Price);
        }

        if (query.Sizes != null && query.Sizes.Any())
        {
            predicate = predicate.And(p => p.Sizes != null && p.Sizes.Any(size => query.Sizes.Contains(size.Name)));
        }

        return predicate;
    }
}
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

    public async Task<IList<Product>> GetProductsByQuery(GetProductsSearch query, CancellationToken cancellationToken)
    {
        var predicate = BuildPredicate(query);
        var allProduct = await _context.Products.ToListAsync(cancellationToken);
        return await _context.Products.Include(p => p.Sizes).Where(predicate).ToListAsync(cancellationToken);
    }
    
    private static Expression<Func<Product, bool>> BuildPredicate(GetProductsSearch query)
    {
        var predicate = PredicateBuilder.True<Product>();

        if (query.MaxPrice > 0)
        {
            predicate = predicate.And(p => p.Price <= query.MaxPrice);
        }
        if (query.MinPrice > 0)
        {
            predicate = predicate.And(p => p.Price <= query.MinPrice);
        }

        if (query.Sizes != null && query.Sizes.Any())
        {
            predicate = predicate.And(p => p.Sizes != null && p.Sizes.Any(size => query.Sizes.Contains(size.Name)));
        }

        return predicate;
    }
}
using Products.Application.Products.Queries.GetProductsFilter;
using Products.Domain;

namespace Products.Application.Interfaces;

public interface IProductRepository
{
    public Task<IList<Product>> GetAllProducts(CancellationToken cancellationToken);
    public Task AddProduct(Product model, CancellationToken cancellationToken);
    public Task AddProducts(IList<Product> model, CancellationToken cancellationToken);
    public Task<IList<Product>> GetProductsByQuery(GetProductsFilterQuery query, CancellationToken cancellationToken);
}
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Products.Application.Common.Mapping;
using Products.Application.Interfaces;
using Products.Application.Products.Queries.GetProductsFilter;
using Products.Persistence;

namespace Products.Test.Common;

public class QueryTestFixture : IDisposable
{
    private ProductDbContext _context;
    public IMapper Mapper;
    public ProductRepository Repository;
    public ProductFilterAnalyzer FilterAnalyzer;
    private MemoryCache _memoryCache;

    public QueryTestFixture()
    {
        _context = ProductDbContextFactory.Create();
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AssemblyMappingProfile(
                typeof(IProductDbContext).Assembly));
        });
        Mapper = configurationProvider.CreateMapper();
        Repository = new ProductRepository(_context);
        
        // Setting up MemoryCache
        var memoryCacheOptions = new MemoryCacheOptions();
        _memoryCache = new MemoryCache(memoryCacheOptions);
        
        // Instantiating ProductFilterAnalyzer
        FilterAnalyzer = new ProductFilterAnalyzer(Repository, _memoryCache);

    }

    public void Dispose()
    {
        ProductDbContextFactory.Destroy(_context);
    }
}

[CollectionDefinition("QueryCollection")]
public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
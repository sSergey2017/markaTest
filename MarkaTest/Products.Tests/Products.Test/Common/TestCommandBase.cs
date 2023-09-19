using Products.Persistence;

namespace Products.Test.Common;

public abstract class TestCommandBase : IDisposable
{
    private readonly ProductDbContext _context;
    protected readonly ProductRepository Repository;

    public TestCommandBase()
    {
        _context = ProductDbContextFactory.Create();
        Repository = new ProductRepository(_context);
    }

    public void Dispose()
    {
        ProductDbContextFactory.Destroy(_context);
    }
}
using Products.Application.Common.Models;

namespace Products.Application.Interfaces;

public interface IProductFilterAnalyzer
{
    public Task <ProductFilterAnalise> CreateFullAnalise(CancellationToken cancellationToken);
    public Task InvalidateAndRebuildCache(CancellationToken cancellationToken);
    
}
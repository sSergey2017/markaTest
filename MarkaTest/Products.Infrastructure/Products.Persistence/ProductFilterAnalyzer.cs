using Microsoft.Extensions.Caching.Memory;
using Products.Application.Common.Models;
using Products.Application.Interfaces;

namespace Products.Application.Products.Queries.GetProductsFilter;

public class ProductFilterAnalyzer : IProductFilterAnalyzer
{
    private readonly IMemoryCache _cache;
    private readonly IProductRepository _repository;

    public ProductFilterAnalyzer(IProductRepository repository, IMemoryCache cache)
    {
        _cache = cache;
        _repository = repository;
    }

    public async Task<ProductFilterAnalise> CreateFullAnalise(CancellationToken cancellationToken)
    {
        if (!_cache.TryGetValue("ProductFilterAnalysis", out ProductFilterAnalise filter))
        {
            var products = await _repository.GetAllProducts(cancellationToken);

            filter = new ProductFilterAnalise
            {
                PriceRange = new PriceRange
                {
                    MinPrice = products.DefaultIfEmpty().Min(p => p?.Price ?? 0),
                    MaxPrice = products.DefaultIfEmpty().Max(p => p?.Price ?? 0)
                },
                Sizes = products.SelectMany(p => p.Sizes).Select(s => s.Name).Distinct().ToList(),
                CommonWords = GetTopWords(products.Select(p => p.Description), 10, 5)
            };

            _cache.Set("ProductFilterAnalysis", filter, TimeSpan.FromHours(1));
        }

        return filter;
    }
    
    public async Task InvalidateAndRebuildCache(CancellationToken cancellationToken)
    {
        _cache.Remove("ProductFilterAnalysis");
        var newAnalysis = await CreateFullAnalise(cancellationToken);
        _cache.Set("ProductFilterAnalysis", newAnalysis, TimeSpan.FromHours(1));
    }

    private List<string> GetTopWords(IEnumerable<string> descriptions, int topN, int excludeTopN)
    {
        var wordCount = new Dictionary<string, int>();

        foreach (var description in descriptions)
        {
            var words = description.Split(new[] { ' ', ',', '.', '!', '?', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                var lowerWord = word.ToLower();
                if (wordCount.ContainsKey(lowerWord))
                {
                    wordCount[lowerWord]++;
                }
                else
                {
                    wordCount[lowerWord] = 1;
                }
            }
        }
        
        var excludeWords = wordCount.OrderByDescending(w => w.Value).Take(excludeTopN).Select(w => w.Key).ToList();

        var topWords = wordCount
            .Where(w => !excludeWords.Contains(w.Key))
            .OrderByDescending(w => w.Value)
            .Take(topN)
            .Select(w => w.Key)
            .ToList();

        return topWords;
    }

}
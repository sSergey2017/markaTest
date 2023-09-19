using AutoMapper;
using Polly.Retry;
using Products.Application.Interfaces;
using Products.Domain;
using Products.Persistence.Models;
using Serilog;

namespace Products.Persistence;

public class ProductInitializationService : BaseService, IProductInitializationService
{
    private readonly IProductRepository _repository;
    private IProductFilterAnalyzer _analyzer;
    private readonly AsyncRetryPolicy _retryPolicy;


    public ProductInitializationService(IHttpClientFactory httpClientFactory, 
        IProductRepository repository, 
        IProductFilterAnalyzer analyzer, 
        RetryPolicyProvider retryPolicyProvider) 
        : base(httpClientFactory)
    {
        _repository = repository;
        _analyzer = analyzer;
        _retryPolicy = retryPolicyProvider.GetDefaultRetryPolicy();
    }

    public async Task InitializeProduct()
    {
        CancellationToken token = new CancellationToken();
        var res = await GetAllProducts<RootDTO>();
        Log.Information("Products List: {@Res}", res);
        var data = ConvertProducts(res);
        await _repository.AddProducts(data, token);
        await _analyzer.InvalidateAndRebuildCache(token);
    }

    private List<Product> ConvertProducts(RootDTO res)
    {
        List<Product> productsData = new();
        var productsDto = res.Products;

        foreach (var productDto in productsDto)
        {
            var product = new Product
            {
                Title = productDto.Title,
                Price = productDto.Price,
                Description = productDto.Description
            };

            product.Sizes = ConvertStringSizesToSizeEntities(productDto.Sizes, product);

            productsData.Add(product);
        }

        return productsData;
    }
    
    private List<Size> ConvertStringSizesToSizeEntities(List<string>? sizes, Product product)
    {
        if (sizes == null)
        {
            return new List<Size>();
        }

        return sizes.Select(s => new Size { Name = s, Product = product }).ToList();
    }


    private async Task<T> GetAllProducts<T>()
    {
        return await _retryPolicy.ExecuteAsync(async () => await SendAsync<T>(new ApiRequest
        {
            ApiType = SD.ApiType.GET,
            Url = SD.MockyAPIBase,
        }));
    }
}
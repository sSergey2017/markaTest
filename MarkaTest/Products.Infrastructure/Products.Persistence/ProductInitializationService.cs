using AutoMapper;
using Products.Application.Interfaces;
using Products.Domain;
using Products.Persistence.Models;

namespace Products.Persistence;

public class ProductInitializationService : BaseService, IProductInitializationService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;
    private readonly HttpClient _httpClient;

    public ProductInitializationService(IHttpClientFactory httpClientFactory, IMapper mapper, IProductRepository repository) : base(httpClientFactory)
    {
        _mapper = mapper;
        _repository = repository;
        _httpClient = httpClientFactory.CreateClient("MockyAPI");
    }

    public async Task InitializeProduct()
    {
        var res = await GetAllProducts<Root>();
        Console.WriteLine(res);
    }
    
    private async Task<T> GetAllProducts<T>()
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.MockyAPIBase,
        });
    }
}
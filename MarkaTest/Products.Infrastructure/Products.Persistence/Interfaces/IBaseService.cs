using Products.Persistence.Models;

namespace Products.Persistence.Interfaces;

public interface IBaseService: IDisposable
{
    ResponseDto ResponseModel { get; set; }
    Task<T> SendAsync<T>(ApiRequest apiRequest);
}
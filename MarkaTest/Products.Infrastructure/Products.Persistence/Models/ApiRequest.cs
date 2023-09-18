namespace Products.Persistence.Models;
using static Products.Persistence.SD;

public class ApiRequest
{
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
}

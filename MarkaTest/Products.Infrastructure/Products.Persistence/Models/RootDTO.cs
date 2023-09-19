namespace Products.Persistence.Models;

public class ApiKeysDTO
{
    public string? Primary { get; set; }
    public string? Secondary { get; set; }
}

public class ProductDTO
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int Price { get; set; }
    public List<string>? Sizes { get; set; } = new List<string>();
    public string? Description { get; set; }
}

public class RootDTO
{
    public List<ProductDTO>? Products { get; set; }
    public ApiKeysDTO? ApiKeys { get; set; }
}
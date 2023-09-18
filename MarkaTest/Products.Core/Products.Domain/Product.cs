namespace Products.Domain;

public class ApiKeys
{
    public string? Primary { get; set; }
    public string? Secondary { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int Price { get; set; }
    public List<Size>? Sizes { get; set; } = new List<Size>();
    public string? Description { get; set; }
}
public class Size
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProductId { get; set; }  // Foreign key for Product
    public Product Product { get; set; }
}

public class Root
{
    public List<Product>? Products { get; set; }
    public ApiKeys? ApiKeys { get; set; }
}
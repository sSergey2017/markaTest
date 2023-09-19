using Products.Domain;

namespace Products.Application.Common.Models;

public class ProductFilterAnalise
{
    public PriceRange PriceRange { get; set; }
    public List<string> Sizes { get; set; }
    public List<string> CommonWords { get; set; }
}

public class PriceRange
{
    public int MinPrice { get; set; }
    public int MaxPrice { get; set; }
}
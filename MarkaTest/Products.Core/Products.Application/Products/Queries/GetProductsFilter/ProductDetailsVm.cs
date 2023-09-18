using AutoMapper;
using Products.Application.Common.Mapping;
using Products.Domain;

namespace Products.Application.Products.Queries;

public class ProductDetailsVm : IMapWith<Product>
{
    public string? Title { get; set; }
    public int Price { get; set; }
    public List<string>? Sizes { get; set; }
    public string? Description { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductDetailsVm>()
            .ForMember(dest => dest.Sizes, 
            opt => opt.MapFrom(src => src.Sizes.Select(s => s.Name).ToList()));

    }
}
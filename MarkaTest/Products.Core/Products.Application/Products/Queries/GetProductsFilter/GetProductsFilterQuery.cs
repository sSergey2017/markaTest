using System.Text.RegularExpressions;
using AutoMapper;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using Products.Application.Common.Exceptions;
using Products.Application.Common.Models;
using Products.Application.Interfaces;
using Products.Domain;

namespace Products.Application.Products.Queries.GetProductsFilter;

public class ProductionListVm
{
    public IList<ProductDetailsVm>? Products { get; set; } 
    public ProductFilterAnalise Analise  { get; set; }
}

public class GetProductsFilterQuery : IRequest<ProductionListVm>
{
    public int? MaxPrice { get; set; }
    public int? MinPrice { get; set; }
    public string? Sizes { get; set; }
    public string? Highlight { get; set; }
}

public class GetProductsSearch 
{
    public int? MaxPrice { get; set; }
    public int? MinPrice { get; set; }
    public List<string>? Sizes { get; set; }
    public List<string>? Highlight { get; set; }
}

public class GetProductsFilterQueryHandler : IRequestHandler<GetProductsFilterQuery, ProductionListVm>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;
    private readonly IProductFilterAnalyzer _analyzer;

    public GetProductsFilterQueryHandler(IProductRepository repository, IMapper mapper, IProductFilterAnalyzer analyzer) 
        => (_repository, _mapper, _analyzer) = (repository, mapper, analyzer);
    

    public async Task<ProductionListVm> Handle(GetProductsFilterQuery request, CancellationToken cancellationToken)
    {
        GetProductsSearch search = CreateSearchQuery(request);
        var entity = await _repository.GetProductsByQuery(search, cancellationToken);
        if (entity == null)
        {
            string requestJson = JsonConvert.SerializeObject(request, Formatting.Indented);
            throw new NotFoundException(nameof(Product), requestJson);
        }
        var res = _mapper.Map<IList<ProductDetailsVm>>(entity);

        if (search.Highlight != null && search.Highlight.Any())
        {
            foreach (var product in res)
            {
                product.Description = HighlightWordsInDescription(product.Description, search.Highlight);
            }
        }

        ProductFilterAnalise analise = await _analyzer.CreateFullAnalise(cancellationToken);
        return new ProductionListVm {Products = res, Analise = analise};
    }

    private string? HighlightWordsInDescription(string? description, List<string> wordsToHighlight)
    {
        foreach (var word in wordsToHighlight)
        {
            description = Regex.Replace(description, $@"\b{word}\b", $"<em>{word}</em>", RegexOptions.IgnoreCase);
        }
        return description;
    }

    private GetProductsSearch CreateSearchQuery(GetProductsFilterQuery query)
    {
        return new GetProductsSearch
        {
            MaxPrice = query.MaxPrice,
            MinPrice = query.MinPrice,
            Sizes = query.Sizes?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            Highlight = query.Highlight?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
        };
    }
}

public class CreateGetProductsFilterQueryValidator : AbstractValidator<GetProductsFilterQuery>
{
    public CreateGetProductsFilterQueryValidator()
    {
        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxPrice.HasValue);
        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinPrice.HasValue);
        RuleFor(x => x.Sizes)
            .Custom((sizes, context) => {
                var sizeList = sizes?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var size in sizeList)
                {
                    if (!Enum.TryParse<SizeEnum>(size, true, out var _))
                    {
                        context.AddFailure($"Size '{size}' is not a valid size. Valid sizes are Small, Medium, Large.");
                    }
                }
            }).When(x => !string.IsNullOrWhiteSpace(x.Sizes));
    }
}
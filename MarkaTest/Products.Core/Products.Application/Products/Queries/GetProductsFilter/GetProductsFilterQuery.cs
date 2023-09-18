using AutoMapper;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using Products.Application.Common.Exceptions;
using Products.Application.Interfaces;
using Products.Domain;

namespace Products.Application.Products.Queries.GetProductsFilter;

public class ProductionListVm
{
    public IList<ProductDetailsVm>? Products { get; set; } 
}

public class GetProductsFilterQuery : IRequest<ProductionListVm>
{
    public int? Price { get; set; }
    public List<string>? Sizes { get; set; }
}

public class GetProductsFilterQueryHandler : IRequestHandler<GetProductsFilterQuery, ProductionListVm>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;

    public GetProductsFilterQueryHandler(IProductRepository repository, IMapper mapper) => (_repository, _mapper) = (repository, mapper);
    

    public async Task<ProductionListVm> Handle(GetProductsFilterQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetProductsByQuery(request, cancellationToken);
        if (entity == null)
        {
            string requestJson = JsonConvert.SerializeObject(request, Formatting.Indented);
            throw new NotFoundException(nameof(Product), requestJson);
        }
        var res = _mapper.Map<IList<ProductDetailsVm>>(entity);

        return new ProductionListVm {Products = res};
    }
}

public class CreateGetProductsFilterQueryValidator : AbstractValidator<GetProductsFilterQuery>
{
    public CreateGetProductsFilterQueryValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Price.HasValue);
        RuleForEach(x => x.Sizes)
            .Where(size => !string.IsNullOrEmpty(size))
            .Length(0, 20)
            .WithMessage("Each size must be 20 characters or less.");
    }
}
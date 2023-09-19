using AutoMapper;
using Products.Application.Interfaces;
using Products.Application.Products.Queries.GetProductsFilter;
using Products.Test.Common;
using Shouldly;

namespace Products.Test.Queries;

[Collection("QueryCollection")]
public class GetProductsFilterQueryTests
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;
    private readonly IProductFilterAnalyzer _analyzer;

    public GetProductsFilterQueryTests(QueryTestFixture fixture)
    {
        _mapper = fixture.Mapper;
        _repository = fixture.Repository;
        _analyzer = fixture.FilterAnalyzer;
    }
    
    [Fact]
    public async Task GetEventListQueryHandler_Success()
    {
        // Arrange
        var handler = new GetProductsFilterQueryHandler(_repository, _mapper, _analyzer);

        // Act
        var result = await handler.Handle(
            new GetProductsFilterQuery
                { },
            CancellationToken.None);

        // Assert
        result.ShouldBeOfType<ProductionListVm>();
        result.Products.Count.ShouldBe(5);
    }
    
    [Fact]
    public async Task GetEventListQueryHandler_Search_3()
    {
        // Arrange
        var handler = new GetProductsFilterQueryHandler(_repository, _mapper, _analyzer);

        // Act
        var result = await handler.Handle(
            new GetProductsFilterQuery
            {
                Sizes = "small",
                MaxPrice = 20
            },
            CancellationToken.None);

        // Assert
        result.ShouldBeOfType<ProductionListVm>();
        result.Products.Count.ShouldBe(3);
    }
    
    [Fact]
    public async Task GetEventListQueryHandler_Search_1()
    {
        // Arrange
        var handler = new GetProductsFilterQueryHandler(_repository, _mapper, _analyzer);

        // Act
        var result = await handler.Handle(
            new GetProductsFilterQuery
            {
                Sizes = "small",
                MinPrice = 14,
                Highlight = "blue, green"
            },
            CancellationToken.None);

        // Assert
        result.ShouldBeOfType<ProductionListVm>();
        result.Products.Count.ShouldBe(1);
        var product = result.Products.First();
        product.Title.ShouldBe("A Green Trouser");
        product.Price.ShouldBe(14);
        product.Description.ShouldBe("This trouser perfectly pairs with a <em>blue</em> shirt.");
        product.Sizes.ShouldContain("small");
        product.Sizes.ShouldContain("medium");
    }
}
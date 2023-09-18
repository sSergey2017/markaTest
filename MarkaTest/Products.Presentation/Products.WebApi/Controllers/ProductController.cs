using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Products.Queries.GetProductsFilter;

namespace Products.WebApi.Controllers;

[Route("api/[controller]")]
public class ProductController : BaseController
{
    private readonly IMapper _mapper;

    public ProductController(IMapper mapper) => _mapper = mapper;
    
    [HttpGet]
    public async Task<ActionResult<ProductionListVm>> GetAll()
    {
        var query = new GetProductsFilterQuery
        {
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }
    
    [HttpGet("filter")]
    public async Task<ActionResult<ProductionListVm>> FilterProducts([FromQuery] GetProductsFilterQuery query)
    {
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }
}
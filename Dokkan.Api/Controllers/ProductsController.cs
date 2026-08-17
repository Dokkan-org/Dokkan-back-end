using Dokkan.Api.Abstractions;
using Dokkan.Api.Contracts.Common;
using Dokkan.Api.Contracts.Products;
using Dokkan.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dokkan.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet()]
    public async Task<IActionResult> GetAll([FromQuery] ProductFilters productFilters, CancellationToken cancellationToken)
    {
        var result= await _productService.GetAllAsync(productFilters, cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable([FromQuery] ProductFilters productFilters, CancellationToken cancellationToken)
    {
        var result= await _productService.GetAvailableAsync( productFilters, cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute]int id, CancellationToken cancellationToken)
    {
        var result = await _productService.GetAsync(id, cancellationToken);
        return result.IsSuccess?
            Ok(result.Value):
            result.ToProblem();
    }

    [HttpPost()]
    public async Task<IActionResult> Add([FromBody]ProductRequest request, CancellationToken cancellationToken)
    {
        var result= await _productService.AddAsync( request, cancellationToken);
        return result.IsSuccess ?
            CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) :
            result.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute]int id, [FromBody]ProductRequest request, CancellationToken cancellationToken)
    {
        var result= await _productService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ?
            NoContent() :
            result.ToProblem();
    }

    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute]int id, CancellationToken cancellationToken)
    {
        var result= await _productService.ToggleActiveAsync(id, cancellationToken);
        return result.IsSuccess ?
            NoContent() :
            result.ToProblem();
    }

}
           
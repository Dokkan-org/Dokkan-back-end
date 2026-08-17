using Dokkan.Api.Abstractions;
using Dokkan.Api.Contracts.Brand;
using Dokkan.Api.Contracts.Common;
using Dokkan.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dokkan.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandsController(IBrandService service) : ControllerBase
{
    private readonly IBrandService _brandService = service;

    [HttpGet()]
    public async Task<IActionResult> GetAll([FromQuery]RequestFilters request,CancellationToken cancellationToken)
    {
        var result= await _brandService.GetAllAsync(request, cancellationToken);
        return Ok(result.Value);
    }
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable([FromQuery]RequestFilters request,CancellationToken cancellationToken)
    {
        var result= await _brandService.GetAvailableAsync(request, cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute]int id,CancellationToken cancellationToken)
    {
        var result= await _brandService.GetAsync(id, cancellationToken);
        return result.IsSuccess ?
            Ok(result.Value) :
            result.ToProblem();
    }

    [HttpPost()]
    public async Task<IActionResult> Add([FromBody]BrandRequest request,CancellationToken cancellationToken)
    {
        var result= await _brandService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ?
            CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value):
            result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,[FromBody]BrandRequest request,CancellationToken cancellationToken)
    {
        var result= await _brandService.UpdateAsync(id,request, cancellationToken);
        return result.IsSuccess ?
           NoContent():
           result.ToProblem();
    }

    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(int id,CancellationToken cancellationToken)
    {
        var result= await _brandService.ToggleActiveStatus(id, cancellationToken);
        return result.IsSuccess ?
           NoContent():
           result.ToProblem();
    }


}

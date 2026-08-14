using Dokkan.Api.Abstractions;
using Dokkan.Api.Contracts.Category;
using Dokkan.Api.Contracts.Common;
using Dokkan.Api.Services;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dokkan.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryService service) : ControllerBase
{
    private readonly ICategoryService _categoryService = service;

    [HttpGet()]
    public async Task<IActionResult> GetAll([FromQuery]RequestFilters request,bool? isActive,CancellationToken cancellationToken)
    {
        var result= await _categoryService.GetAllAsync(request,isActive, cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute]int id,CancellationToken cancellationToken)
    {
        var result= await _categoryService.GetAsync(id, cancellationToken);
        return result.IsSuccess ?
            Ok(result.Value) :
            result.ToProblem();
    }

    [HttpPost()]
    public async Task<IActionResult> Add([FromBody]CategoryRequest request,CancellationToken cancellationToken)
    {
        var result= await _categoryService.CreateAsync(request, cancellationToken);
        return result.IsSuccess ?
            CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value):
            result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,[FromBody]CategoryRequest request,CancellationToken cancellationToken)
    {
        var result= await _categoryService.UpdateAsync(id,request, cancellationToken);
        return result.IsSuccess ?
           NoContent():
           result.ToProblem();
    }

    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(int id,CancellationToken cancellationToken)
    {
        var result= await _categoryService.ToggleActiveStatus(id, cancellationToken);
        return result.IsSuccess ?
           NoContent():
           result.ToProblem();
    }


}

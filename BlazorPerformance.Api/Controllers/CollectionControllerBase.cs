using BlazorPerformance.Shared.Models;
using BlazorPerformance.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlazorPerformance.Api.Controllers;

public abstract class CollectionControllerBase<T> : Controller
{
    private readonly IDataService<T> _dataService;

    public CollectionControllerBase(IDataService<T> dataService)
    {
        _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
    }

    [HttpGet]
    [HttpHead]
    public async Task<IActionResult> GetCollectionAsync([FromQuery] string? searchTerm, [FromQuery] int skip = 0, [FromQuery] int take = 1000
        , CancellationToken cancellationToken = default)
    {
        var result = await _dataService.GetCollectionAsync(new CollectionRequest { Skip = skip, Take = take, SearchTerm = searchTerm ?? string.Empty }, cancellationToken);
        Response.Headers["X-Collection-Count"] = $"{result.Count}";
        Response.Headers["Access-Control-Expose-Headers"] = "X-Collection-Count";
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItemAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var result = await _dataService.GetItemAsync(id, cancellationToken);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }
}

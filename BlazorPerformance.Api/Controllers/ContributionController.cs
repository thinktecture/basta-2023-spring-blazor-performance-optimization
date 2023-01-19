using Microsoft.AspNetCore.Mvc;
using BlazorPerformance.Api.Services;
using BlazorPerformance.Shared.Services;
using BlazorPerformance.Shared.Models;

namespace BlazorPerformance.Api.Controllers;

[Route("[controller]")]
public class ContributionController : CollectionControllerBase<Contribution>
{
    private readonly ContributionsService _contributionsService;

    public ContributionController(ContributionsService contributionsService)
        : base(contributionsService)
    {
        _contributionsService = contributionsService;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItemAsync([FromRoute] int id, [FromBody] Contribution contribution, CancellationToken cancellationToken = default)
    {
        var result = await _contributionsService.GetItemAsync(id, cancellationToken);
        if (result == null)
        {
            return NotFound();
        }

        await _contributionsService.UpdateContributionAsync(id, contribution, cancellationToken);

        return Ok();
    }
}

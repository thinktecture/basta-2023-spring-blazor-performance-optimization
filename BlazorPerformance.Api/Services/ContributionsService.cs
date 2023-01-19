using Microsoft.EntityFrameworkCore;
using BlazorPerformance.Shared.Models;
using BlazorPerformance.Shared.Services;
using BlazorPerformance.Api.Data;

namespace BlazorPerformance.Api.Services;

public class ContributionsService : IContributionsService
{
    private readonly SampleDatabaseContext _context;

    public ContributionsService(SampleDatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public Task<List<Contribution>> GetCollectionAsync(CollectionRequest request, CancellationToken cancellationToken)
    {
        var result = String.IsNullOrWhiteSpace(request.SearchTerm)
            ? _context.Contributions
            : _context.Contributions.Where(c => c.Title.Contains(request.SearchTerm));
        return result.Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
    }

    public Task<Contribution?> GetItemAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Contributions.FirstOrDefaultAsync(contribution => contribution.Id == id, cancellationToken);
    }

    public async Task UpdateContributionAsync(int id, Contribution contribution, CancellationToken cancellationToken)
    {
        var currentContribution = await _context.Contributions.FirstOrDefaultAsync(contribution => contribution.Id == id, cancellationToken);
        if (currentContribution != null)
        {
            currentContribution.Title = contribution.Title;
            currentContribution.PrimaryTag = contribution.PrimaryTag;
            currentContribution.Abstract = contribution.Abstract;
            currentContribution.Type = contribution.Type;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

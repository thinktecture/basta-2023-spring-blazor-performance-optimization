using Microsoft.EntityFrameworkCore;
using BlazorPerformance.Shared.Models;
using BlazorPerformance.Shared.Services;
using BlazorPerformance.Api.Data;

namespace BlazorPerformance.Api.Services;

public class ConferencesService : IConferenceService
{
    private readonly SampleDatabaseContext _context;

    public ConferencesService(SampleDatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<List<Conference>> GetCollectionAsync(CollectionRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(500);
        var result = string.IsNullOrWhiteSpace(request.SearchTerm)
            ? _context.Conferences
            : _context.Conferences.Where(c => c.Title.Contains(request.SearchTerm));
        return await result.Skip(request.Skip).Take(request.Take).OrderBy(c => c.Id).ToListAsync(cancellationToken);
    }

    public Task<Conference?> GetItemAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Conferences.FirstOrDefaultAsync(conf => conf.Id == id, cancellationToken);
    }

    public async Task<Conference?> GetRandomConference(int lastId, CancellationToken cancellationToken)
    {
        var random = new Random();
        var conferenceCount = _context.Conferences.Count();
        if (conferenceCount > 0)
        {
            int randomIndex = lastId;
            while (randomIndex == lastId || randomIndex <= 0)
            {
                randomIndex = random.Next(1, conferenceCount > 10 ? 10 : conferenceCount);
            }
            return await _context.Conferences.FirstOrDefaultAsync(c => c.Id == randomIndex, cancellationToken);
        }
        return null;
    }
}

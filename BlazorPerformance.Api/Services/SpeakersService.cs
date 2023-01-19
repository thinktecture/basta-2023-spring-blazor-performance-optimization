using Microsoft.EntityFrameworkCore;
using BlazorPerformance.Shared.Models;
using BlazorPerformance.Shared.Services;
using BlazorPerformance.Api.Data;

namespace BlazorPerformance.Api.Services;

public class SpeakersService : ISpeakersService
{
    private readonly SampleDatabaseContext _context;

    public SpeakersService(SampleDatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public Task<List<Speaker>> GetCollectionAsync(CollectionRequest request, CancellationToken cancellationToken)
    {
        var result = String.IsNullOrWhiteSpace(request.SearchTerm)
            ? _context.Speakers
            : _context.Speakers.Where(c => c.FirstName.Contains(request.SearchTerm) || c.LastName.Contains(request.SearchTerm));
        return result.Skip(request.Skip).Take(request.Take).ToListAsync();
    }

    public Task<Speaker?> GetItemAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Speakers.FirstOrDefaultAsync(speaker => speaker.Id == id, cancellationToken);
    }
}

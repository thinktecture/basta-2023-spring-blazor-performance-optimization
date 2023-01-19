using BlazorPerformance.Shared.Models;
using System.ServiceModel;

namespace BlazorPerformance.Shared.Services;

[ServiceContract]
public interface IContributionsService : IDataService<Contribution>
{
    Task UpdateContributionAsync(int id, Contribution contribution, CancellationToken cancellationToken);
}

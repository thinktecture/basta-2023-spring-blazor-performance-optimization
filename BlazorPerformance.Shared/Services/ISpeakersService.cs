using BlazorPerformance.Shared.Services;
using BlazorPerformance.Shared.Models;
using System.ServiceModel;

namespace BlazorPerformance.Shared.Services;

[ServiceContract]
public interface ISpeakersService : IDataService<Speaker>
{
}

using BlazorPerformance.Shared.Models;
using System.ServiceModel;

namespace BlazorPerformance.Shared.Services;

[ServiceContract]
public interface IConferenceService : IDataService<Conference>
{
}

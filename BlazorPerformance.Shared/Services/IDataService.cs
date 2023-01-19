using System.Runtime.Serialization;
using System.ServiceModel;

namespace BlazorPerformance.Shared.Services;

[ServiceContract]
public interface IDataService<T>
{
    [OperationContract]
    Task<List<T>> GetCollectionAsync(CollectionRequest request, CancellationToken cancellationToken);

    [OperationContract]
    Task<T?> GetItemAsync(int id, CancellationToken cancellationToken);
}

[DataContract]
public class CollectionRequest
{
    [DataMember(Order = 1)]
    public int Skip { get; set; }

    [DataMember(Order = 2)]
    public int Take { get; set; }

    [DataMember(Order = 3)]
    public string SearchTerm { get; set; }
}

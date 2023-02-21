using System.Runtime.Serialization;

namespace BlazorPerformance.Shared.Models;

[DataContract]
public class Root<T>
{
    [DataMember(Order = 1)]
    public List<T> Items { get; set; }
    [DataMember(Order = 2)]
    public int ItemCount { get; set; }
}

using BlazorPerformance.Shared.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace BlazorPerformance.Client.Services;

public class DataService<T>
    where T : class, IModelId
{
    private readonly HttpClient _httpClient;

    public DataService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<List<T>> GetCollectionAsync(int skip, int take, string searchTerm, CancellationToken cancellationToken)
    {
        try
        {
            var query = new Dictionary<string, string>()
            {
                ["searchTerm"] = searchTerm,
                ["skip"] = $"{skip}",
                ["take"] = $"{take}"
            };

            var uri = QueryHelpers.AddQueryString($"{_httpClient.BaseAddress}{typeof(T).Name}", query);
            
            var type = typeof(T).Name.ToLower();
            var response = await _httpClient.GetFromJsonAsync<List<T>>(
                uri,
                cancellationToken);
            return response ?? new List<T>();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Load data from api failed. Error {e.Message}");
            throw e ?? default!;
        }
    }

    public async Task<int> GetCollectionCountAsync(string searchTerm = "", CancellationToken cancellationToken = default)
    {
        var type = typeof(T).Name.ToLower();
        var request = new HttpRequestMessage(HttpMethod.Head, string.IsNullOrWhiteSpace(searchTerm) ? $"{type}" : $"{type}?searchTerm={searchTerm}");

        using var httpResponse = await _httpClient.SendAsync(request, cancellationToken);
        var countHeader = httpResponse.Headers.GetValues("X-Collection-Count")?.FirstOrDefault();
        var count = 0;
        if (!string.IsNullOrWhiteSpace(countHeader))
        {
            int.TryParse(countHeader, out count);
        }
        return count;
    }

    public async Task<T?> GetItemAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var type = typeof(T).Name.ToLower();
            var response = await _httpClient.GetFromJsonAsync<T>(
                $"{type}/{id}",
                cancellationToken);
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Fail to load data item. Error {e.Message}");
            return null;
        }
    }

    public Task UpdateItemAsync(T item, CancellationToken cancellationToken = default)
    {
        var type = typeof(T).Name.ToLower();
        return _httpClient.PutAsJsonAsync($"{type}/{item.Id}", item, cancellationToken);
    }

    public Task RemoveItemAsync(int contributionId, CancellationToken cancellationToken = default)
    {
        var type = typeof(T).Name.ToLower();
        return _httpClient.DeleteAsync($"{type}/{contributionId}", cancellationToken);
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using BlazorPerformance.Shared.Models;
using BlazorPerformance.Client.Services;
using System.Diagnostics;

namespace BlazorPerformance.Client.Components;

public partial class Collection<TItem>
    where TItem : class, IModelId
{
    [Inject] public DataService<TItem> DataService { get; set; } = default!;

    #region Parameter

    [Parameter]
    public ICollection<TItem> Items { get; set; } = new List<TItem>();

    [Parameter]
    public string SearchTerm { get; set; } = string.Empty;
    [Parameter]
    public bool Searching { get; set; }

    [Parameter]
    public RenderFragment HeaderContent { get; set; } = default!;

    [Parameter]
    public RenderFragment<TItem> RowContent { get; set; } = default!;

    [Parameter]
    public RenderFragment RowPlaceholder { get; set; } = default!;
    #endregion

    private Virtualize<TItem>? _virtualize;
    private string _searchTerm = string.Empty;
    private int _renderingTimes = 0;
    private Stopwatch renderingStopwatch = new Stopwatch();

    protected override void OnInitialized()
    {
        renderingStopwatch.Reset();
        renderingStopwatch.Start();
    }

    protected override void OnParametersSet()
    {
        renderingStopwatch.Reset();
        renderingStopwatch.Start();
        base.OnParametersSet();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
        {
            _renderingTimes++;
        }
        if (renderingStopwatch.IsRunning)
        {
            renderingStopwatch.Stop();
            StateHasChanged();
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_searchTerm != SearchTerm)
        {
            _searchTerm = SearchTerm;
            await ReloadCollection();
        }

        await base.OnParametersSetAsync();
    }

    public async Task ReloadCollection()
    {
        if (_virtualize != null)
        {
            await _virtualize.RefreshDataAsync();
        }
    }

    private async ValueTask<ItemsProviderResult<TItem>> LoadCollection(
        ItemsProviderRequest request)
    {
        try
        {
            var count = await DataService.GetCollectionCountAsync(SearchTerm, request.CancellationToken);

            var totalCount = Math.Min(request.Count, count - request.StartIndex);
            var collection =
                await DataService.GetCollectionAsync(request.StartIndex, totalCount, SearchTerm,
                    request.CancellationToken);
            return new ItemsProviderResult<TItem>(collection, count);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Current request was canceled.");
            return new ItemsProviderResult<TItem>(new List<TItem>(), 0);
        }
    }


}

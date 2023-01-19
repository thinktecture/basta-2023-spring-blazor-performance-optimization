using BlazorPerformance.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorPerformance.Client.Services;

namespace BlazorPerformance.Client.Components;

public abstract class CollectionBaseComponent<T, TDialogComponent> : ComponentBase
    where T : class, IModelId
    where TDialogComponent : ComponentBase
{
    [Inject] public DataService<T> DataService { get; set; } = default!;
    [Inject] public SignalRService SignalRService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    [Parameter] public bool VirtualizeEnabled { get; set; }
    [Parameter] public bool WithItemsProvider { get; set; }
    [Parameter] public string SearchTerm { get; set; } = string.Empty;

    protected List<T> Collection = new List<T>();
    protected Collection<T>? CollectionComponent;
    protected bool IsLoading = false;
    protected virtual int TakeCount => 1000;
    private string _searchTerm = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        SignalRService.CountChanged += SignalRService_CountChanged;
        await SignalRService.InitConnectionAsync();

        Collection = await DataService.GetCollectionAsync(0, TakeCount, SearchTerm, CancellationToken.None);

        IsLoading = false;
        await base.OnInitializedAsync();
    }

    private void SignalRService_CountChanged(object? sender, ConferenceCountEventArgs e)
    {
        UpdateCollection(e.Id, e.Count);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (SearchTerm != _searchTerm)
        {
            _searchTerm = SearchTerm;
            Collection = await DataService.GetCollectionAsync(0, 1000, SearchTerm, CancellationToken.None);
            await InvokeAsync(StateHasChanged);
        }
        await base.OnParametersSetAsync();
    }

    protected async Task CollectionItemClicked(int id)
    {
        var parameters = new DialogParameters();
        parameters.Add("Id", id);
        parameters.Add("PreventRendering", VirtualizeEnabled);
        var dialogOptions = new DialogOptions() { FullWidth = true, MaxWidth = MaxWidth.ExtraLarge, CloseButton = true, DisableBackdropClick = true };
        var dialogRef = DialogService.Show<TDialogComponent>($"{typeof(T).Name} Editor", parameters, dialogOptions);
        var result = await dialogRef.Result;
        if (!result.Cancelled && result.Data is T speaker)
        {
            if (!WithItemsProvider)
            {
                Collection = await DataService.GetCollectionAsync(0, 1000, string.Empty, CancellationToken.None);
            }
            else if (CollectionComponent != null)
            {
                await CollectionComponent.ReloadCollection();
            }
            StateHasChanged();
        }
    }

    protected abstract void UpdateCollection(int id, int count);
}

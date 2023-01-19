using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Services;

namespace BlazorPerformance.Client.Pages;

public partial class Index
{
    [Inject] public IBreakpointService BreakpointService { get; set; } = default!;

    private Guid _subscriptionId;
    private Position activePosition = Position.Left;
    private bool _virtualize = false;
    private bool _loadWithItemsProvider = false;
    private string _searchTerm = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var subscriptionResult = await BreakpointService.Subscribe((breakpoint) =>
            {
                activePosition = breakpoint < Breakpoint.Sm ? Position.Top : Position.Left;
                InvokeAsync(StateHasChanged);
            }, new ResizeOptions
            {
                ReportRate = 250,
                NotifyOnBreakpointOnly = true,
            });

            activePosition = subscriptionResult.Breakpoint < Breakpoint.Sm ? Position.Top : Position.Left;
            _subscriptionId = subscriptionResult.SubscriptionId;
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    public async ValueTask DisposeAsync() => await BreakpointService.Unsubscribe(_subscriptionId);
}

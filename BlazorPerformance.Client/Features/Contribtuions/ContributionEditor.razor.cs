using BlazorPerformance.Client.Services;
using BlazorPerformance.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorPerformance.Client.Features.Contribtuions
{
    public partial class ContributionEditor
    {
        [Inject] public DataService<Contribution> ContributionsService { get; set; } = default!;
        [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public int Id { get; set; }
        [Parameter] public bool PreventRendering { get; set; } = false;

        private Contribution? _contribution;

        protected override async Task OnInitializedAsync()
        {
            _contribution = await ContributionsService.GetItemAsync(Id, CancellationToken.None);
            await base.OnInitializedAsync();
        }

        private async Task Submit()
        {
            if (_contribution != null)
            {
                await ContributionsService.UpdateItemAsync(_contribution);
                MudDialog.Close(DialogResult.Ok(_contribution));
            }
        }
        void Cancel() => MudDialog.Cancel();
    }
}
using BlazorPerformance.Client.Services;
using BlazorPerformance.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorPerformance.Client.Features.Conferences
{
    public partial class ConferenceEditor
    {
        [Inject] public DataService<Conference> ConferencesService { get; set; } = default!;
        [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public int Id { get; set; }
        [Parameter] public bool PreventRendering { get; set; }

        private Conference? _conference;

        protected override async Task OnInitializedAsync()
        {
            _conference = await ConferencesService.GetItemAsync(Id, CancellationToken.None);
            await base.OnInitializedAsync();
        }

        private async Task Submit()
        {
            if (_conference != null)
            {
                await ConferencesService.UpdateItemAsync(_conference);
                MudDialog.Close(DialogResult.Ok(_conference));
            }
        }
        void Cancel() => MudDialog.Cancel();
    }
}
using BlazorPerformance.Shared.Models;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BlazorPerformance.Client.Features.Conferences
{
    public partial class Conferences
    {
        private int _idToUpdate = 0;
        private int _count = 0;

        protected override void UpdateCollection(int id, int count)
        {
            _idToUpdate = id;
            _count = count;
            InvokeAsync(StateHasChanged);
        }
    }
}
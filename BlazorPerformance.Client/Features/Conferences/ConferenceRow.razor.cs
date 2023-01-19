using BlazorPerformance.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorPerformance.Client.Features.Conferences
{
    public partial class ConferenceRow
    {
        [Parameter] public Conference Conference { get; set; } = default!;
        [Parameter] public int HighlightId { get; set; }
        [Parameter] public int Count { get; set; }

        private string _hightlightClass = string.Empty;
        private bool _shouldRender = true;

        protected override bool ShouldRender() => _shouldRender;

        protected override void OnParametersSet()
        {
            if (Conference?.Id == HighlightId)
            {
                Conference.VisitorsCount = Count;
                _hightlightClass = "highlight";
                _shouldRender = true;
            }
            else
            {
                _shouldRender = !string.IsNullOrWhiteSpace(_hightlightClass);
                _hightlightClass = string.Empty;
            }
            base.OnParametersSet();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Console.WriteLine($"Conference rendered: {Conference.Id}");
            base.OnAfterRender(firstRender);
        }
    }
}
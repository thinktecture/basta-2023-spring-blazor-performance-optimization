using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPerformance.Client.Components
{
    public partial class SearchBar : IDisposable
    {
        [Inject] public IJSRuntime JS { get; set; }

        [Parameter] public string SearchTerm { get; set; } = string.Empty;
        [Parameter] public string Title { get; set; } = string.Empty;

        [Parameter] public EventCallback<string> SearchTermChanged { get; set; }


        private IJSObjectReference? _module;
        private DotNetObjectReference<SearchBar>? _selfReference;
        private ElementReference _searchBarElement;
        private int _valueHashCode;

        [JSInvokable]
        public void HandleOnInput(string value)
        {
            Console.WriteLine($"TextChanged {SearchTerm}. JS Value {value}");
            if (SearchTerm != value)
            {
                SearchTerm = value;
                SearchTermChanged.InvokeAsync(SearchTerm);
                StateHasChanged();
            }
        }

        protected override bool ShouldRender()
        {
            var lastHashCode = _valueHashCode;
            _valueHashCode = SearchTerm?.GetHashCode() ?? 0;
            return _valueHashCode != lastHashCode;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    Console.WriteLine("Register debounce JS Event");
                    _selfReference = DotNetObjectReference.Create(this);
                    var minInterval = 500; // Only notify every 500 ms
                    if (_module == null)
                    {
                        _module = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/SearchBar.razor.js");
                    }
                    await _module.InvokeVoidAsync("onDebounceInput",
                        _searchBarElement, _selfReference, minInterval);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void Dispose() => _selfReference?.Dispose();
    }
}

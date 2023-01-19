using BlazorPerformance.Client.Services;
using BlazorPerformance.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace BlazorPerformance.Client.Features.Contribtuions
{
    public partial class Contributions
    {
        [Inject] public DataService<Contribution> _dataService { get; set; } = default!;

        private bool _showList = true;
        private int _numResults;
        GridItemsProvider<Contribution> _itemsProvider;
        private GridSort<Contribution> titleSort = GridSort<Contribution>.ByAscending(x => x.Title);

        protected override void OnInitialized()
        {
            pagination.TotalItemCountChanged += (sender, eventArgs) => StateHasChanged();
            _itemsProvider = async request =>
            {
                var totalItemCount = await _dataService.GetCollectionCountAsync(SearchTerm, request.CancellationToken);
                var result = await _dataService.GetCollectionAsync(request.StartIndex, request.Count ?? 0, SearchTerm, request.CancellationToken);

                // Separately display the item count
                if (totalItemCount != _numResults && !request.CancellationToken.IsCancellationRequested)
                {
                    _numResults = totalItemCount;
                    StateHasChanged();
                }

                return new GridItemsProviderResult<Contribution>(result, totalItemCount);
            };
            base.OnInitialized();
        }

        protected override void UpdateCollection(int id, int count)
        {
        }

        private void ToggleView()
        {
            _showList = !_showList;
            StateHasChanged();
        }

        #region Paging
        PaginationState pagination = new() { ItemsPerPage = 50 };

        private async Task GoToPageAsync(int pageIndex)
        {
            await pagination.SetCurrentPageIndexAsync(pageIndex);
        }

        private string? PageButtonClass(int pageIndex)
            => pagination.CurrentPageIndex == pageIndex ? "current" : null;

        private string? AriaCurrentValue(int pageIndex)
            => pagination.CurrentPageIndex == pageIndex ? "page" : null;
        #endregion
    }
}
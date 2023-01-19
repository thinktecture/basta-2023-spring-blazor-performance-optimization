using Microsoft.AspNetCore.Components;

namespace BlazorPerformance.Client.Components.Forms
{
    public partial class HandleEventInputText : IHandleEvent
    {
        [Parameter] public string Id { get; set; } = string.Empty;
        [Parameter] public string Label { get; set; } = string.Empty;

        private bool _preventRender;
        private string? _renderMessage;
        private int _renderCount = 0;

        #region Optimierung
        private int _valueHashCode;
        private bool _shouldRender = true;

        protected override bool ShouldRender() => _shouldRender;

        protected override void OnParametersSet()
        {
            var lastHashCode = _valueHashCode;
            _valueHashCode = CurrentValue?.GetHashCode() ?? 0;
            var shouldRender = _valueHashCode != lastHashCode;
            _shouldRender = shouldRender;
            base.OnParametersSet();
        }

        #endregion

        protected override void OnAfterRender(bool firstRender)
        {
            _renderCount++;
            _renderMessage = $"{typeof(HandleEventInputText).Name}.{Label}: Render by ShouldRender. RenderCount: {_renderCount} times.";
            base.OnAfterRender(firstRender);
        }

        #region Optimierung2
        public Task HandleEventAsync(EventCallbackWorkItem item, object? arg)
        {
            try
            {
                var task = item.InvokeAsync(arg);
                var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion &&
                                      task.Status != TaskStatus.Canceled;

                if (!_preventRender)
                {
                    StateHasChanged();
                }

                return shouldAwaitTask
                    ? CallStateHasChangedOnAsyncCompletion(task, _preventRender)
                    : Task.CompletedTask;
            }
            finally
            {
                _preventRender = false;
            }
        }

        private async Task CallStateHasChangedOnAsyncCompletion(Task task, bool preventRender)
        {
            try
            {
                await task;
            }
            catch
            {
                if (task.IsCanceled)
                {
                    return;
                }

                throw;
            }

            if (!preventRender)
            {
                StateHasChanged();
            }
        }

        void PreventRender()
        {
            CurrentValue = Value;
            _preventRender = true;
        }
        #endregion

        protected override bool TryParseValueFromString(string value, out string result,
            out string validationErrorMessage)
        {
            result = value;
            validationErrorMessage = string.Empty;
            return true;
        }
    }
}
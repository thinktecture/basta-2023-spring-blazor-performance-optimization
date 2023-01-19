using Microsoft.AspNetCore.Components;
using System;

namespace BlazorPerformance.Client.Components.Forms
{
    public partial class CustomInput
    {
        [Parameter, EditorRequired] public string Label { get; set; } = string.Empty;

        private string? _renderMessage;
        private int _renderCount = 0;

        #region Optimierung
        private int _valueHashCode;
        private bool _shouldRender = true;

        protected override bool ShouldRender() => _shouldRender;

        protected override void OnParametersSet()
        {
            var lastHashCode = _valueHashCode;
            _valueHashCode = Value?.GetHashCode() ?? 0;
            var shouldRender = _valueHashCode != lastHashCode;
            _shouldRender = shouldRender;
            base.OnParametersSet();
        }

        #endregion

        protected override void OnAfterRender(bool firstRender)
        {
            _renderCount++;
            _renderMessage = $"{typeof(CustomInput).Name}.{Label}: Render by ShouldRender. RenderCount: {_renderCount} times.";
            base.OnAfterRender(firstRender);
        }

        protected override bool TryParseValueFromString(string value, out string result,
            out string validationErrorMessage)
        {
            result = value;
            validationErrorMessage = null;
            return true;
        }
    }
}

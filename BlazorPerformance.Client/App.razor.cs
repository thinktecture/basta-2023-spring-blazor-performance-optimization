using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorPerformance.Client
{
    public partial class App
    {
        [JSInvokable]
        public static async Task HandleJSException(JavaScriptException exception)
        {
            Console.WriteLine($"JS Exception logged in .NET: {JsonSerializer.Serialize(exception)}");
        }
    }

    public class JavaScriptException
    {
        public string ErrorMessage { get; set; }
        public string Level { get; set; }
        public string LogDestination { get; set; }
    }
}
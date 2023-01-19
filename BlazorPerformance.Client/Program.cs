using BlazorPerformance.Client;
using BlazorPerformance.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7231") });
builder.Services.AddScoped(typeof(DataService<>));
builder.Services.AddScoped(typeof(SignalRService));
builder.Services.AddMudServices();

await builder.Build().RunAsync();

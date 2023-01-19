using BlazorPerformance.Api.BackgroundServices;
using BlazorPerformance.Api.Data;
using BlazorPerformance.Api.Hubs;
using BlazorPerformance.Api.Services;
using BlazorPerformance.Api.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProtoBuf.Grpc.Server;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(sp => new HttpClient());
builder.Services.AddScoped<ContributionsService>();
builder.Services.AddScoped<ConferencesService>();
builder.Services.AddScoped<SpeakersService>();

builder.Services.AddDbContext<SampleDatabaseContext>(
                options => options.UseInMemoryDatabase(databaseName: "BlazorPerformance"));

builder.Services.AddControllers();

builder.Services.AddCodeFirstGrpc(config => { config.ResponseCompressionLevel = CompressionLevel.Optimal; });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Net6Features.Api", Version = "v1" });
});

builder.Services.AddSignalR();
builder.Services.AddHostedService<RealTimeSyncService>();

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor.WASM.Api v1"));
}

// ONLY FOR DEMO
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DataGenerator.InitializeAsync(services);
}

app.UseCors(corsBuilder => corsBuilder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .SetIsOriginAllowed(host => true));

app.UseHttpsRedirection();
app.UseRouting();

app.UseGrpcWeb();


app.MapGrpcService<ContributionsService>().EnableGrpcWeb();
app.MapGrpcService<ConferencesService>().EnableGrpcWeb();
app.MapGrpcService<SpeakersService>().EnableGrpcWeb();
app.MapHub<RealtimeHub>("/count");
app.MapControllers();

app.Run();
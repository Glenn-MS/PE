using System.Reflection;
using System.Text.Json;
using AzureDevOpsProvider;
using AzureProvider;
using Core;
using GitHubProvider;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add Application Insights telemetry
builder.Services.AddApplicationInsightsTelemetry();

// Add controller support
builder.Services.AddControllers();

// Configure API Explorer for OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure HTTP clients with resilience policies
builder.Services.AddHttpClient("GitHubClient")
    .AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

builder.Services.AddHttpClient("AzureDevOpsClient")
    .AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

// Register core services
builder.Services.AddSingleton<IProvider, AzureProvider.AzureProvider>();
builder.Services.AddSingleton<IProvider, GitHubProvider.GitHubProvider>(sp => 
    new GitHubProvider.GitHubProvider(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("GitHubClient"),
        sp.GetRequiredService<ILogger<GitHubProvider.GitHubProvider>>()));
builder.Services.AddSingleton<IProvider, AzureDevOpsProvider.AzureDevOpsProvider>(sp => 
    new AzureDevOpsProvider.AzureDevOpsProvider(
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("AzureDevOpsClient"),
        sp.GetRequiredService<ILogger<AzureDevOpsProvider.AzureDevOpsProvider>>()));

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck("AzureProvider", () => HealthCheckResult.Healthy())
    .AddCheck("GitHubProvider", () => HealthCheckResult.Healthy())
    .AddCheck("AzureDevOpsProvider", () => HealthCheckResult.Healthy());

// Load client extension assemblies dynamically
var clientExtensionsPath = Path.Combine(AppContext.BaseDirectory, "ClientExtensions");
if (Directory.Exists(clientExtensionsPath))
{
    foreach (var dll in Directory.GetFiles(clientExtensionsPath, "*.dll"))
    {
        var assembly = Assembly.LoadFrom(dll);
        var extensionTypes = assembly.GetTypes().Where(t => typeof(IProvider).IsAssignableFrom(t) && !t.IsInterface);

        foreach (var extensionType in extensionTypes)
        {
            builder.Services.AddSingleton(typeof(IProvider), extensionType);
        }
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

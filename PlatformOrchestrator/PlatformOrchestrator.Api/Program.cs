using Azure.Identity;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PlatformOrchestrator.Api.Middleware;
using PlatformOrchestrator.Infrastructure;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add Application Insights telemetry
if (!string.IsNullOrEmpty(builder.Configuration["ApplicationInsights:ConnectionString"]))
{
    builder.Services.AddApplicationInsightsTelemetry(options =>
    {
        options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    });
    
    // Configure Application Insights telemetry processor
    builder.Services.AddSingleton<ITelemetryInitializer, CloudRoleNameTelemetryInitializer>();
}

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register infrastructure services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck<PlatformOrchestrator.Api.Health.OrchestratorHealthCheck>("orchestrator_health");

builder.Services.AddSingleton<IUserService, UserService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://login.microsoftonline.com/{tenantId}";
        options.Audience = "{clientId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://sts.windows.net/{tenantId}/",
            ValidateAudience = true,
            ValidAudience = "{clientId}",
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Add custom request monitoring middleware
app.UseMiddleware<RequestMonitoringMiddleware>();

// Enable CORS
app.UseCors();

// Add authentication middleware
app.UseMiddleware<AuthenticationMiddleware>();

// Add authorization middleware
app.UseMiddleware<AuthorizationMiddleware>();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    AllowCachingResponses = false
});

app.Run();

// Telemetry Initializer to set cloud role name
internal class CloudRoleNameTelemetryInitializer : ITelemetryInitializer
{
    public void Initialize(Microsoft.ApplicationInsights.Channel.ITelemetry telemetry)
    {
        // Set cloud role name to distinguish this component in Application Insights
        telemetry.Context.Cloud.RoleName = "PlatformOrchestrator";
    }
}

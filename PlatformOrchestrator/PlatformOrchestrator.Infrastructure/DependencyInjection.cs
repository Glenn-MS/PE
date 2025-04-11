using Azure.Identity;
using Azure.ResourceManager;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlatformOrchestrator.Core.Services;
using PlatformOrchestrator.Infrastructure.Services;

namespace PlatformOrchestrator.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds infrastructure services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add Azure clients
        services.AddAzureClients(clientBuilder =>
        {
            // Add ARM client using DefaultAzureCredential
            clientBuilder.AddArmClient(new Uri(configuration["Azure:ArmEndpoint"] ?? "https://management.azure.com/"))
                .WithCredential(GetAzureCredential(configuration));
            
            // Add Key Vault client if configured
            var keyVaultUri = configuration["Azure:KeyVault:Uri"];
            if (!string.IsNullOrEmpty(keyVaultUri))
            {
                clientBuilder.AddSecretClient(new Uri(keyVaultUri))
                    .WithCredential(GetAzureCredential(configuration));
            }
                
            // Configure retry policies
            clientBuilder.UseHttpClientFactory(httpClientBuilder =>
            {
                httpClientBuilder.ConfigureHttpClient(client =>
                {
                    client.Timeout = TimeSpan.FromMinutes(5);
                });
            });
            
            // Configure global retry policy
            clientBuilder.ConfigureRetries(builder =>
            {
                builder.MaxRetries = 5;
                builder.MaxRetryDelay = TimeSpan.FromSeconds(30);
            });
        });
        
        // Register orchestration service
        services.AddScoped<IOrchestrationService, AzureOrchestrationService>();
        
        // Register secret service if Key Vault is configured
        if (!string.IsNullOrEmpty(configuration["Azure:KeyVault:Uri"]))
        {
            services.AddScoped<ISecretService, KeyVaultSecretService>();
        }
        
        // Register GitHub provider
        services.AddScoped<IProvider, GitHubProvider>();
        
        return services;
    }return services;
    }
    
    /// <summary>
    /// Gets the appropriate Azure credential based on the environment
    /// </summary>
    private static DefaultAzureCredential GetAzureCredential(IConfiguration configuration)
    {
        var options = new DefaultAzureCredentialOptions
        {
            ExcludeSharedTokenCacheCredential = true,
            ExcludeVisualStudioCredential = false,
            ExcludeVisualStudioCodeCredential = false,
            ExcludeAzureCliCredential = false,
            ExcludeInteractiveBrowserCredential = true,
            ExcludeManagedIdentityCredential = false
        };
        
        return new DefaultAzureCredential(options);
    }
}

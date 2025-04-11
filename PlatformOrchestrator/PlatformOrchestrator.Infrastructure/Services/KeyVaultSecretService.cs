using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlatformOrchestrator.Core.Services;
using System.Threading.Tasks;

namespace PlatformOrchestrator.Infrastructure.Services;

/// <summary>
/// Implementation of the secret management service using Azure Key Vault
/// </summary>
public class KeyVaultSecretService : ISecretService
{
    private readonly SecretClient _secretClient;
    private readonly ILogger<KeyVaultSecretService> _logger;

    public KeyVaultSecretService(IConfiguration configuration, ILogger<KeyVaultSecretService> logger)
    {
        _logger = logger;
        
        // Get Key Vault URI from configuration
        var keyVaultUri = configuration["Azure:KeyVault:Uri"] 
            ?? throw new ArgumentNullException("Azure:KeyVault:Uri", "Key Vault URI is not configured");
        
        // Create a secret client using Managed Identity when available, or DefaultAzureCredential
        _secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
    }

    /// <inheritdoc />
    public async Task<string> GetSecretAsync(string secretName)
    {
        try
        {
            _logger.LogInformation("Retrieving secret {SecretName} from Key Vault", secretName);
            
            var response = await _secretClient.GetSecretAsync(secretName);
            return response.Value.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving secret {SecretName} from Key Vault", secretName);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task SetSecretAsync(string secretName, string secretValue)
    {
        try
        {
            _logger.LogInformation("Setting secret {SecretName} in Key Vault", secretName);
            
            await _secretClient.SetSecretAsync(secretName, secretValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting secret {SecretName} in Key Vault", secretName);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteSecretAsync(string secretName)
    {
        try
        {
            _logger.LogInformation("Deleting secret {SecretName} from Key Vault", secretName);
            
            await _secretClient.StartDeleteSecretAsync(secretName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting secret {SecretName} from Key Vault", secretName);
            throw;
        }
    }
}

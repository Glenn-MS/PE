namespace PlatformOrchestrator.Core.Services;

/// <summary>
/// Service for managing secrets securely
/// </summary>
public interface ISecretService
{
    /// <summary>
    /// Gets a secret by name
    /// </summary>
    /// <param name="secretName">Name of the secret</param>
    /// <returns>Secret value</returns>
    Task<string> GetSecretAsync(string secretName);
    
    /// <summary>
    /// Sets a secret value
    /// </summary>
    /// <param name="secretName">Name of the secret</param>
    /// <param name="secretValue">Value of the secret</param>
    Task SetSecretAsync(string secretName, string secretValue);
    
    /// <summary>
    /// Deletes a secret
    /// </summary>
    /// <param name="secretName">Name of the secret</param>
    Task DeleteSecretAsync(string secretName);
}

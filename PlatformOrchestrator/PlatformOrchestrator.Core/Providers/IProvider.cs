namespace PlatformOrchestrator.Core.Providers;

/// <summary>
/// Interface for provider integrations (e.g., GitHub, Azure Dev Center)
/// </summary>
public interface IProvider
{
    /// <summary>
    /// Gets the name of the provider
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Retrieves metadata for a specific resource
    /// </summary>
    /// <param name="resourceId">The ID of the resource</param>
    /// <returns>Metadata as a dictionary</returns>
    Task<Dictionary<string, object>> GetResourceMetadataAsync(string resourceId);

    /// <summary>
    /// Executes an operation on a resource
    /// </summary>
    /// <param name="resourceId">The ID of the resource</param>
    /// <param name="operation">The operation to execute</param>
    /// <param name="parameters">Additional parameters for the operation</param>
    /// <returns>Operation result</returns>
    Task<string> ExecuteOperationAsync(string resourceId, string operation, Dictionary<string, object>? parameters = null);
}

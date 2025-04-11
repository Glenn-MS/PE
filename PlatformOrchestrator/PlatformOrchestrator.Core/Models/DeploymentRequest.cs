namespace PlatformOrchestrator.Core.Models;

public class DeploymentRequest
{
    /// <summary>
    /// Name for the resource to deploy
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Type of resource to deploy (e.g., virtualmachine, storageaccount, etc.)
    /// </summary>
    public string ResourceType { get; set; } = string.Empty;
    
    /// <summary>
    /// Azure region where to deploy the resource
    /// </summary>
    public string Region { get; set; } = string.Empty;
    
    /// <summary>
    /// Deployment parameters specific to the resource type
    /// </summary>
    public Dictionary<string, object>? Parameters { get; set; }
    
    /// <summary>
    /// Resource tags for organization and governance
    /// </summary>
    public Dictionary<string, string>? Tags { get; set; }
}

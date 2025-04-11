namespace PlatformOrchestrator.Core.Models;

public class Resource
{
    /// <summary>
    /// Unique identifier for the resource
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Name of the resource
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Type of the resource (e.g., virtualmachine, storageaccount, etc.)
    /// </summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// Azure region where the resource is deployed
    /// </summary>
    public string Region { get; set; } = string.Empty;
    
    /// <summary>
    /// Current status of the resource (e.g., Running, Stopped, etc.)
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Resource-specific properties as a dictionary
    /// </summary>
    public Dictionary<string, object>? Properties { get; set; }
    
    /// <summary>
    /// Resource tags for organization and governance
    /// </summary>
    public Dictionary<string, string>? Tags { get; set; }
    
    /// <summary>
    /// When the resource was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// When the resource was last updated
    /// </summary>
    public DateTime? LastUpdatedAt { get; set; }
}

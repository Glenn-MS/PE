namespace PlatformOrchestrator.Core.Models;

public class DeploymentStatus
{
    /// <summary>
    /// Unique identifier for the deployment
    /// </summary>
    public string DeploymentId { get; set; } = string.Empty;
    
    /// <summary>
    /// Current status of the deployment (e.g., Pending, InProgress, Succeeded, Failed)
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Detailed message about the deployment status
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// ID of the resource that was deployed (if successful)
    /// </summary>
    public string? ResourceId { get; set; }
    
    /// <summary>
    /// When the deployment was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// When the deployment status was last updated
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }
}

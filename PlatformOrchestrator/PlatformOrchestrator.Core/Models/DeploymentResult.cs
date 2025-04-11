namespace PlatformOrchestrator.Core.Models;

public class DeploymentResult
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
    /// When the deployment was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

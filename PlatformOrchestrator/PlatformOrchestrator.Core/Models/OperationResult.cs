namespace PlatformOrchestrator.Core.Models;

public class OperationResult
{
    /// <summary>
    /// Unique identifier for the operation
    /// </summary>
    public string OperationId { get; set; } = string.Empty;
    
    /// <summary>
    /// ID of the resource the operation was performed on
    /// </summary>
    public string ResourceId { get; set; } = string.Empty;
    
    /// <summary>
    /// Current status of the operation (e.g., Pending, InProgress, Succeeded, Failed)
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Detailed message about the operation result
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// When the operation was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

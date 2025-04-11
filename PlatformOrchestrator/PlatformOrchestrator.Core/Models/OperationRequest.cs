namespace PlatformOrchestrator.Core.Models;

public class OperationRequest
{
    /// <summary>
    /// ID of the resource to perform the operation on
    /// </summary>
    public string ResourceId { get; set; } = string.Empty;
    
    /// <summary>
    /// Type of operation to perform (e.g., start, stop, restart, delete)
    /// </summary>
    public string OperationType { get; set; } = string.Empty;
    
    /// <summary>
    /// Additional parameters for the operation
    /// </summary>
    public Dictionary<string, object>? Parameters { get; set; }
}

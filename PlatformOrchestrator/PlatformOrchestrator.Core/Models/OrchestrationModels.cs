namespace PlatformOrchestrator.Core.Models;

/// <summary>
/// Represents a request to deploy a resource
/// </summary>
public class DeploymentRequest
{
    /// <summary>
    /// Type of resource to deploy (e.g., VM, Container, Function, etc.)
    /// </summary>
    public required string ResourceType { get; set; }
    
    /// <summary>
    /// Name for the deployed resource
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Region to deploy the resource to
    /// </summary>
    public required string Region { get; set; }
    
    /// <summary>
    /// Configuration parameters for the deployment
    /// </summary>
    public Dictionary<string, string> Parameters { get; set; } = new();
    
    /// <summary>
    /// Tags to apply to the deployed resource
    /// </summary>
    public Dictionary<string, string> Tags { get; set; } = new();
}

/// <summary>
/// Represents the result of a deployment operation
/// </summary>
public class DeploymentResult
{
    /// <summary>
    /// Unique identifier for the deployment
    /// </summary>
    public required string DeploymentId { get; set; }
    
    /// <summary>
    /// Current status of the deployment
    /// </summary>
    public required string Status { get; set; }
    
    /// <summary>
    /// Resource ID of the deployed resource (if available)
    /// </summary>
    public string? ResourceId { get; set; }
    
    /// <summary>
    /// Timestamp when the deployment was initiated
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents the status of a deployment
/// </summary>
public class DeploymentStatus
{
    /// <summary>
    /// Unique identifier for the deployment
    /// </summary>
    public required string DeploymentId { get; set; }
    
    /// <summary>
    /// Current status of the deployment
    /// </summary>
    public required string Status { get; set; }
    
    /// <summary>
    /// Resource ID of the deployed resource (if available)
    /// </summary>
    public string? ResourceId { get; set; }
    
    /// <summary>
    /// Detailed message about the deployment status
    /// </summary>
    public string? Message { get; set; }
    
    /// <summary>
    /// Timestamp when the deployment was initiated
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Timestamp when the deployment was last updated
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }
}

/// <summary>
/// Represents a resource managed by the platform orchestrator
/// </summary>
public class Resource
{
    /// <summary>
    /// Unique identifier for the resource
    /// </summary>
    public required string Id { get; set; }
    
    /// <summary>
    /// Name of the resource
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Type of the resource
    /// </summary>
    public required string Type { get; set; }
    
    /// <summary>
    /// Region where the resource is deployed
    /// </summary>
    public required string Region { get; set; }
    
    /// <summary>
    /// Current status of the resource
    /// </summary>
    public required string Status { get; set; }
    
    /// <summary>
    /// Tags associated with the resource
    /// </summary>
    public Dictionary<string, string> Tags { get; set; } = new();
    
    /// <summary>
    /// Timestamp when the resource was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Represents a request to perform an operation on a resource
/// </summary>
public class OperationRequest
{
    /// <summary>
    /// Type of operation to perform (e.g., Start, Stop, Restart, etc.)
    /// </summary>
    public required string OperationType { get; set; }
    
    /// <summary>
    /// ID of the resource to perform the operation on
    /// </summary>
    public required string ResourceId { get; set; }
    
    /// <summary>
    /// Additional parameters for the operation
    /// </summary>
    public Dictionary<string, string> Parameters { get; set; } = new();
}

/// <summary>
/// Represents the result of an operation
/// </summary>
public class OperationResult
{
    /// <summary>
    /// Unique identifier for the operation
    /// </summary>
    public required string OperationId { get; set; }
    
    /// <summary>
    /// Status of the operation
    /// </summary>
    public required string Status { get; set; }
    
    /// <summary>
    /// ID of the resource the operation was performed on
    /// </summary>
    public required string ResourceId { get; set; }
    
    /// <summary>
    /// Detailed message about the operation
    /// </summary>
    public string? Message { get; set; }
    
    /// <summary>
    /// Timestamp when the operation was initiated
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

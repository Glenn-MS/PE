using PlatformOrchestrator.Core.Models;

namespace PlatformOrchestrator.Core.Services;

/// <summary>
/// Service for orchestrating platform resources and operations
/// </summary>
public interface IOrchestrationService
{
    /// <summary>
    /// Create a new deployment
    /// </summary>
    /// <param name="request">Deployment request</param>
    /// <returns>Deployment result</returns>
    Task<DeploymentResult> CreateDeploymentAsync(DeploymentRequest request);
    
    /// <summary>
    /// Get the status of a deployment
    /// </summary>
    /// <param name="deploymentId">ID of the deployment</param>
    /// <returns>Deployment status</returns>
    Task<DeploymentStatus> GetDeploymentStatusAsync(string deploymentId);
      /// <summary>
    /// Get resources managed by the orchestrator with filtering and pagination
    /// </summary>
    /// <param name="resourceType">Optional resource type filter</param>
    /// <param name="region">Optional region filter</param>
    /// <param name="status">Optional status filter</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <returns>List of resources</returns>
    Task<IEnumerable<Resource>> GetResourcesAsync(
        string? resourceType = null, 
        string? region = null, 
        string? status = null, 
        int pageSize = 20, 
        int pageNumber = 1);
    
    /// <summary>
    /// Execute an operation on a resource
    /// </summary>
    /// <param name="request">Operation request</param>
    /// <returns>Operation result</returns>
    Task<OperationResult> ExecuteOperationAsync(OperationRequest request);
}

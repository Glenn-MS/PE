using Microsoft.AspNetCore.Mvc;
using PlatformOrchestrator.Core.Services;
using PlatformOrchestrator.Core.Models;
using System.Threading.Tasks;

namespace PlatformOrchestrator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrchestratorController : ControllerBase
{
    private readonly IOrchestrationService _orchestrationService;
    private readonly ILogger<OrchestratorController> _logger;

    public OrchestratorController(
        IOrchestrationService orchestrationService,
        ILogger<OrchestratorController> logger)
    {
        _orchestrationService = orchestrationService;
        _logger = logger;
    }

    [HttpPost("deployments")]
    public async Task<IActionResult> CreateDeployment([FromBody] DeploymentRequest request)
    {
        _logger.LogInformation("Received deployment request for {ResourceType}", request.ResourceType);
        
        try
        {
            var result = await _orchestrationService.CreateDeploymentAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating deployment");
            return StatusCode(500, new { error = "Failed to create deployment", message = ex.Message });
        }
    }

    [HttpGet("deployments/{deploymentId}")]
    public async Task<IActionResult> GetDeploymentStatus(string deploymentId)
    {
        try
        {
            var status = await _orchestrationService.GetDeploymentStatusAsync(deploymentId);
            return Ok(status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving deployment status for {DeploymentId}", deploymentId);
            return StatusCode(500, new { error = "Failed to retrieve deployment status", message = ex.Message });
        }
    }    [HttpGet("resources")]
    public async Task<IActionResult> GetResources(
        [FromQuery] string? resourceType = null,
        [FromQuery] string? region = null,
        [FromQuery] string? status = null,
        [FromQuery] int pageSize = 20,
        [FromQuery] int pageNumber = 1)
    {
        try
        {
            _logger.LogInformation("Retrieving resources with filters: ResourceType={ResourceType}, Region={Region}, Status={Status}, Page={Page}, PageSize={PageSize}",
                resourceType, region, status, pageNumber, pageSize);

            // Add request tracking for monitoring
            Activity.Current?.AddTag("resourceType", resourceType ?? "all");
            
            // Validate pagination parameters
            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new { error = "Page size must be between 1 and 100" });
            }

            if (pageNumber < 1)
            {
                return BadRequest(new { error = "Page number must be greater than 0" });
            }

            // Get resources with filtering and pagination
            var resources = await _orchestrationService.GetResourcesAsync(
                resourceType, 
                region, 
                status, 
                pageSize, 
                pageNumber);

            // Add cache-control headers for improved performance
            Response.Headers.Add("Cache-Control", "private, max-age=60");
            
            return Ok(new
            {
                pageSize,
                pageNumber,
                resources
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving resources");
            return StatusCode(500, new { error = "Failed to retrieve resources", message = ex.Message });
        }
    }

    [HttpPost("operations")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExecuteOperation([FromBody] OperationRequest request)
    {
        _logger.LogInformation("Received operation request: {OperationType} on {ResourceId}", 
            request.OperationType, request.ResourceId);
        
        // Add request tracking for monitoring
        Activity.Current?.AddTag("operationType", request.OperationType);
        Activity.Current?.AddTag("resourceId", request.ResourceId);
        try
        {
            var result = await _orchestrationService.ExecuteOperationAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing operation");
            return StatusCode(500, new { error = "Failed to execute operation", message = ex.Message });
        }
    }
}

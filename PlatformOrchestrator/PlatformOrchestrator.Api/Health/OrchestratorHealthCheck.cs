using Microsoft.Extensions.Diagnostics.HealthChecks;
using PlatformOrchestrator.Core.Services;

namespace PlatformOrchestrator.Api.Health;

/// <summary>
/// Custom health check for the platform orchestrator
/// </summary>
public class OrchestratorHealthCheck : IHealthCheck
{
    private readonly IOrchestrationService _orchestrationService;
    private readonly ILogger<OrchestratorHealthCheck> _logger;

    public OrchestratorHealthCheck(
        IOrchestrationService orchestrationService,
        ILogger<OrchestratorHealthCheck> logger)
    {
        _orchestrationService = orchestrationService;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Performing orchestrator health check");
        
        try
        {
            // Get resources as a simple check that the orchestration service is functioning
            var resources = await _orchestrationService.GetResourcesAsync();
            
            // Additional checks could be added here, such as:
            // - Database connectivity
            // - Azure ARM API availability
            // - Message queue connectivity
            
            return HealthCheckResult.Healthy("Platform orchestrator is healthy");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            return HealthCheckResult.Unhealthy("Platform orchestrator is unhealthy", ex);
        }
    }
}

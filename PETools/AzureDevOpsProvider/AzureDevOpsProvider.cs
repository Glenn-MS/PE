using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Core;
using Microsoft.Extensions.Logging;

namespace AzureDevOpsProvider
{
    /// <summary>
    /// Azure DevOps provider implementation with enhanced security, reliability and monitoring capabilities
    /// </summary>
    public class AzureDevOpsProvider : BaseProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AzureDevOpsProvider> _logger;

        public AzureDevOpsProvider(HttpClient httpClient, ILogger<AzureDevOpsProvider> logger)
            : base(logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            // Configure the HttpClient with sensible defaults
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }
        
        public override string Name => "AzureDevOps";

        protected override async Task<string> ExecuteInternalAsync(string action, params object[] parameters)
        {
            // Log action start with parameters (excluding sensitive data)
            _logger.LogInformation("Starting Azure DevOps action {ActionName} with {ParameterCount} parameters", 
                action, parameters?.Length ?? 0);
            
            try
            {
                // Implement Azure DevOps-specific logic here with proper error handling
                
                switch (action.ToLowerInvariant())
                {
                    case "getproject":
                        if (parameters.Length < 1)
                            throw new ArgumentException("Project name parameter is required");
                        
                        return await GetProjectAsync(parameters[0].ToString());
                        
                    case "getworkitems":
                        if (parameters.Length < 1)
                            throw new ArgumentException("Project name parameter is required");
                        
                        return await GetWorkItemsAsync(parameters[0].ToString());
                        
                    default:
                        _logger.LogWarning("Unknown Azure DevOps action requested: {ActionName}", action);
                        throw new NotSupportedException($"Action '{action}' is not supported");
                }
            }
            catch (HttpRequestException ex)
            {
                // Log network-related exceptions
                _logger.LogError(ex, "Network error during Azure DevOps action {ActionName}: {ErrorMessage}", 
                    action, ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                // Log JSON parsing errors
                _logger.LogError(ex, "JSON parsing error during Azure DevOps action {ActionName}: {ErrorMessage}", 
                    action, ex.Message);
                throw;
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not NotSupportedException)
            {
                // Log other unexpected exceptions
                _logger.LogError(ex, "Unexpected error during Azure DevOps action {ActionName}: {ErrorMessage}", 
                    action, ex.Message);
                throw;
            }
        }
        
        private async Task<string> GetProjectAsync(string projectName)
        {
            _logger.LogInformation("Retrieving Azure DevOps project information: {ProjectName}", projectName);
            
            // For now just simulate the operation
            await Task.Delay(100);
            
            return JsonSerializer.Serialize(new
            {
                Name = projectName,
                Description = "Project description",
                State = "Active",
                Visibility = "Private"
            });
        }
        
        private async Task<string> GetWorkItemsAsync(string projectName)
        {
            _logger.LogInformation("Retrieving Azure DevOps work items for project: {ProjectName}", projectName);
            
            // For now just simulate the operation
            await Task.Delay(100);
            
            return JsonSerializer.Serialize(new[]
            {
                new { Id = 1001, Title = "Sample Task 1", State = "Active", Type = "Task" },
                new { Id = 1002, Title = "Sample Bug 1", State = "Resolved", Type = "Bug" }
            });
        }
    }
}
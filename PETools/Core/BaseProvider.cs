using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Core
{
    /// <summary>
    /// Base implementation of IProvider with retry patterns and error handling
    /// </summary>
    public abstract class BaseProvider : IProvider
    {
        private readonly ILogger _logger;
        private readonly AsyncRetryPolicy _retryPolicy;
        
        protected BaseProvider(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            // Configure retry policy with exponential backoff
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    3, // Retry 3 times
                    attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // Exponential backoff: 2, 4, 8 seconds
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(
                            exception,
                            "Error executing {ProviderName} action (Attempt {RetryCount}). Retrying in {RetryTimeSpan}s.",
                            Name, retryCount, timeSpan.TotalSeconds);
                    });
        }

        public abstract string Name { get; }

        public async Task<string> ExecuteAsync(string action, params object[] parameters)
        {
            try
            {
                _logger.LogInformation("Executing {ProviderName} action: {Action}", Name, action);
                
                // Execute with retry policy
                return await _retryPolicy.ExecuteAsync(
                    async () => await ExecuteInternalAsync(action, parameters));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute {ProviderName} action: {Action}", Name, action);
                throw;
            }
        }
        
        /// <summary>
        /// Internal execution method to be implemented by derived provider classes
        /// </summary>
        protected abstract Task<string> ExecuteInternalAsync(string action, params object[] parameters);
    }
}

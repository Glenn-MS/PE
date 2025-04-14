using Microsoft.Extensions.Logging;

namespace AI
{
    public class AIService
    {
        private readonly HttpClient _httpClient;
        private readonly IKernel _semanticKernel;
        private readonly AutoGenService _autoGenService;
        private readonly ILogger<AIService> _logger;
        private readonly bool _useSemanticKernel;

        public AIService(HttpClient httpClient, IKernel semanticKernel, AutoGenService autoGenService, ILogger<AIService> logger, bool useSemanticKernel = true)
        {
            _httpClient = httpClient;
            _semanticKernel = semanticKernel;
            _autoGenService = autoGenService;
            _logger = logger;
            _useSemanticKernel = useSemanticKernel;
        }

        public async Task<string> ExecuteAgentAsync(string input, Dictionary<string, string> contextVariables)
        {
            try
            {
                _logger.LogInformation("Executing agent with input: {Input}", input);

                if (_useSemanticKernel)
                {
                    var context = new ContextVariables();
                    foreach (var kvp in contextVariables)
                    {
                        context.Set(kvp.Key, kvp.Value);
                    }

                    var result = await _semanticKernel.RunAsync(input, context);
                    return result.Result;
                }
                else
                {
                    return await _autoGenService.GenerateResponseAsync(input, contextVariables);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing agent with input: {Input}", input);
                throw;
            }
        }

        public async Task<string> PerformApiCallAsync(string method, string endpoint, object payload = null)
        {
            try
            {
                _logger.LogInformation("Performing API call: {Method} {Endpoint}", method, endpoint);

                HttpResponseMessage response;
                if (method.ToUpper() == "GET")
                {
                    response = await _httpClient.GetAsync(endpoint);
                }
                else if (method.ToUpper() == "POST")
                {
                    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                    response = await _httpClient.PostAsync(endpoint, content);
                }
                else if (method.ToUpper() == "PUT")
                {
                    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                    response = await _httpClient.PutAsync(endpoint, content);
                }
                else if (method.ToUpper() == "DELETE")
                {
                    response = await _httpClient.DeleteAsync(endpoint);
                }
                else
                {
                    throw new ArgumentException("Unsupported HTTP method");
                }

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing API call: {Method} {Endpoint}", method, endpoint);
                throw;
            }
        }
    }
}
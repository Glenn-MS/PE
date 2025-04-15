using System;
using System.Threading.Tasks;
using AI;
using Api.Controllers;

namespace VisualStudioExtension
{
    public class VisualStudioExtension
    {
        private readonly AIService _aiService;
        private readonly AIController _apiController;

        public VisualStudioExtension(AIService aiService, AIController apiController)
        {
            _aiService = aiService;
            _apiController = apiController;
        }

        public async Task<string> PerformAIAction(string endpoint, object payload)
        {
            return await _aiService.PerformActionAsync(endpoint, payload);
        }

        public async Task<string> CallApiEndpoint(string endpoint, object payload)
        {
            // Example of calling the API layer
            var request = new AIController.AIRequest { Endpoint = endpoint, Payload = payload };
            return await _apiController.PerformAction(request);
        }

        public async Task<string> PerformCombinedAIAndApiAction(string endpoint, object payload)
        {
            // Call AI service
            var aiResult = await _aiService.PerformActionAsync(endpoint, payload);
            // Call API controller
            var apiRequest = new AIController.AIRequest { Endpoint = endpoint, Payload = payload };
            var apiResult = await _apiController.PerformAction(apiRequest);
            // Combine results (simple concatenation, can be customized)
            return $"AI Result: {aiResult}\nAPI Result: {apiResult}";
        }
    }
}
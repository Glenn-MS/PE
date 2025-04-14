using System;
using System.Threading.Tasks;
using AI;
using Api.Controllers;

namespace VSCodeExtension
{
    public class VSCodeExtension
    {
        private readonly AIService _aiService;
        private readonly AIController _apiController;

        public VSCodeExtension(AIService aiService, AIController apiController)
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
    }
}
using Microsoft.AspNetCore.Mvc;
using AI;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly AIService _aiService;

        public AIController(AIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("perform-action")]
        public async Task<IActionResult> PerformAction([FromBody] AIRequest request)
        {
            var result = await _aiService.PerformActionAsync(request.Endpoint, request.Payload);
            return Ok(result);
        }
    }

    public class AIRequest
    {
        public string Endpoint { get; set; }
        public object Payload { get; set; }
    }
}
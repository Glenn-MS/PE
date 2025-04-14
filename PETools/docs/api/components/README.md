# API Components

This document provides detailed descriptions of the components that make up the API layer in the PETools platform.

## Key Components

### 1. Controllers

The API layer uses controllers to expose RESTful endpoints. Each controller corresponds to a specific domain or functionality.

- **AIController**: Handles AI-related operations such as code generation and natural language queries.
  ```csharp
  [ApiController]
  [Route("api/[controller]")]
  public class AIController : ControllerBase
  {
      private readonly IAIService _aiService;

      public AIController(IAIService aiService)
      {
          _aiService = aiService;
      }

      [HttpPost("execute")]
      public async Task<IActionResult> Execute([FromBody] AIRequest request)
      {
          var result = await _aiService.ExecuteOperation(request.OperationName, request.Parameters);
          return Ok(result);
      }
  }
  ```

### 2. Middleware

Middleware components handle cross-cutting concerns such as:

- **Authentication**: Ensures secure access to API endpoints.
- **Logging**: Captures request and response details for auditing and debugging.
- **Error Handling**: Converts exceptions into standardized error responses.

### 3. Dependency Injection

The API layer uses dependency injection to manage service lifetimes and dependencies. Key services include:

- **IAIService**: Provides AI capabilities.
- **IProviderFactory**: Manages provider instances.

### 4. Configuration

Configuration settings for the API layer are stored in `appsettings.json` and `appsettings.Development.json`. These files include:

- API endpoint URLs
- Authentication settings
- Logging configurations

### 5. Filters

Filters are used to apply logic before or after an action method executes. Common filters include:

- **Authorization Filters**: Enforce access control policies.
- **Action Filters**: Validate request data and modify responses.

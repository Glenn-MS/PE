# Platform Orchestrator

A modern, scalable platform orchestrator built with C# and .NET 10 for managing cloud resources and deployments.

## Features

- **Resource Management**: Deploy, monitor, and manage various platform resources
- **Deployment Orchestration**: Coordinate complex deployments across multiple services
- **Operations Execution**: Execute operations on managed resources (start, stop, restart, etc.)
- **Monitoring & Telemetry**: Track request performance and resource status
- **Azure Integration**: Fully integrated with Azure Resource Manager for cloud resource management
- **Authentication & Authorization**: Secure access using MS Entra ID and role-based access control (RBAC)

## Architecture

The solution follows a clean architecture pattern with the following components:

- **PlatformOrchestrator.Api**: Web API project exposing orchestration endpoints
- **PlatformOrchestrator.Core**: Core domain models and service interfaces
- **PlatformOrchestrator.Infrastructure**: Implementation of services using Azure

## Getting Started

### Prerequisites

- .NET 10 SDK
- Azure subscription (for production deployments)
- Visual Studio 2022 or VS Code with C# extension

### Running Locally

1. Clone the repository
2. Update your Azure subscription ID and Key Vault URI in `appsettings.json` and `appsettings.Development.json`
3. Run the application:

```bash
cd PlatformOrchestrator
dotnet restore
dotnet run --project PlatformOrchestrator.Api
```

### API Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/orchestrator/deployments` | POST | Create a new deployment |
| `/api/orchestrator/deployments/{id}` | GET | Get deployment status |
| `/api/orchestrator/resources` | GET | List managed resources |
| `/api/orchestrator/operations` | POST | Execute an operation on a resource |
| `/health` | GET | Health check endpoint |

## Security

This solution follows Azure best practices for security:
- Uses Azure Managed Identity for authentication where possible
- Implements secure credential management with Azure Key Vault
- Supports Key Vault integration for secrets
- Includes proper access control and authorization

## Extending the Platform

To add support for new resource types:
1. Add new models in the Core project
2. Extend the orchestration service in the Infrastructure project
3. Add new endpoints in the API controllers as needed

## License

[MIT](LICENSE)

# Reference Architecture

This document provides a reference architecture for the Platform Engineering Tools (PETools) solution, illustrating how its components interact and align with .NET best practices.

## Overview

The PETools solution is designed with a modular, layered architecture that separates concerns across distinct projects. Each project is responsible for specific functionalities, ensuring scalability, maintainability, and extensibility. The solution follows the .NET 10 clean architecture principles with clear separation of concerns.

## Solution Structure and Layers

The main projects and their roles are:

- **Api**: Exposes RESTful API endpoints for the platform.
- **Core**: Contains core business logic, abstractions, interfaces, and domain models.
- **Infrastructure**: Handles infrastructure-related operations and acts as a factory for various providers.
- **Provider Projects**:
  - **AzureProvider**: Implements Azure-specific functionality.
  - **AzureDevOpsProvider**: Implements Azure DevOps-specific functionality.
  - **GitHubProvider**: Implements GitHub-specific functionality.
- **AI**: Provides AI capabilities and services for intelligent platform features.
- **VSCodeExtension**: Visual Studio Code extension for developer productivity.
- **VisualStudioExtension**: Visual Studio extension for developer productivity.
- **InfrastructureAsCode**: Contains Bicep templates and deployment configurations for Azure resources.

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────────────────────────────┐
│                                    PETools Solution                                          │
│                                                                                             │
│  ┌───────────────┐        ┌───────────────┐        ┌───────────────┐                        │
│  │    Api        │◄───────►│    Core       │◄───────►│ Infrastructure │                        │
│  │  (REST APIs)  │        │(Logic/Models) │        │(Provider Factory)                      │
│  └─────┬─────────┘        └─────┬─────────┘        └─────┬─────────┘                        │
│        │                        │                        │                                  │
│        │                        │                        ▼                                  │
│        │                        │              ┌───────────────────┐                        │
│        │                        │              │   Providers       │                        │
│        │                        │              │                   │                        │
│        │                        │              │ ┌─────────────┐   │                        │
│        │                        │              │ │AzureProvider│   │                        │
│        │                        │              │ └─────────────┘   │                        │
│        │                        │              │                   │                        │
│        │                        │              │ ┌─────────────┐   │                        │
│        │                        │              │ │  GitHub     │   │                        │
│        │                        │              │ │  Provider   │   │                        │
│        │                        │              │ └─────────────┘   │                        │
│        │                        │              │                   │                        │
│        │                        │              │ ┌─────────────┐   │                        │
│        │                        │              │ │Azure DevOps │   │                        │
│        │                        │              │ │  Provider   │   │                        │
│        │                        │              │ └─────────────┘   │                        │
│        │                        │              └───────────────────┘                        │
│        │                        │                                                           │
│        ▼                        ▼                                                           │
│  ┌───────────────┐        ┌───────────────┐        ┌────────────────────┐                   │
│  │     AI        │        │  Extensions   │        │InfrastructureAsCode│                   │
│  │  (Services)   │        │              │        │     (Bicep)         │                   │
│  └───────────────┘        │ ┌───────────┐│        └────────────────────┘                   │
│                           │ │ VS Code   ││                                                  │
│                           │ │ Extension ││                                                  │
│                           │ └───────────┘│                                                  │
│                           │ ┌───────────┐│                                                  │
│                           │ │  Visual   ││                                                  │
│                           │ │  Studio   ││                                                  │
│                           │ │ Extension ││                                                  │
│                           │ └───────────┘│                                                  │
│                           └───────────────┘                                                  │
└─────────────────────────────────────────────────────────────────────────────────────────────┘
```

## Component Roles and Connections

### 1. Api Project
- Exposes RESTful endpoints through controllers (e.g., AIController)
- Handles HTTP requests and responses
- Depends on **Core** for business logic and domain models
- Interacts with **Infrastructure** for provider factory functionality
- Uses **AI** services for AI-powered platform features

### 2. Core Project
- Contains core business logic, domain models, and interfaces
- Defines provider contracts through interfaces (IProvider)
- Provides base abstractions (BaseProvider) for all provider implementations
- Acts as the central hub connecting Api, Infrastructure, and provider implementations

### 3. Infrastructure Project
- Contains the ProviderFactory for creating appropriate provider instances
- Orchestrates the interaction between different provider implementations
- Depends on Core for interfaces and base abstractions
- References all provider-specific projects

### 4. Provider Projects
- **AzureProvider**: Implements Azure-specific functionality and integration
- **GitHubProvider**: Implements GitHub-specific functionality and integration
- **AzureDevOpsProvider**: Implements Azure DevOps-specific functionality and integration
- All implement the IProvider interface defined in Core
- All extend the BaseProvider abstract class defined in Core

### 5. AI Project
- Provides AI services (AIService) for intelligent platform features
- Can be used by Api or directly by client applications
- May leverage providers for AI-related operations

### 6. Extension Projects
- **VSCodeExtension**: Visual Studio Code extension for developer productivity
- **VisualStudioExtension**: Visual Studio extension for developer productivity
- Both interact with the **Api** to provide platform features in the respective IDEs
- May leverage **Core** models for consistency

### 7. InfrastructureAsCode Project
- Contains Bicep templates (main.bicep) for Azure resource deployment
- Defines infrastructure components like App Services, Key Vault, and monitoring resources
- Supports the deployment of the PETools solution to Azure

## Component Interaction Flow

1. **Client/Extension** (VSCodeExtension/VisualStudioExtension) sends HTTP requests to **Api** endpoints.
2. **Api controllers** validate requests and delegate to appropriate services in **Core**.
3. **Core** processes business logic and:
   - Uses **Infrastructure**'s ProviderFactory to get the appropriate provider implementation
   - May leverage **AI** services for intelligent processing
4. **ProviderFactory** in Infrastructure returns the correct provider instance (AzureProvider, GitHubProvider, or AzureDevOpsProvider) based on request context.
5. The selected **Provider** implements Core's interfaces and executes the requested operations against external systems.
6. Results flow back through the layers: Provider → Core → Api → Client/Extension.

## Connection and Dependency Map

- **Api** → **Core**: Uses interfaces and domain models for business logic
- **Api** → **AI**: Integrates AI capabilities into API endpoints
- **Core** ← **Infrastructure**: Infrastructure implements Core's provider interfaces
- **Infrastructure** → **Provider Projects**: References and instantiates specific providers
- **Provider Projects** → **Core**: All providers implement Core interfaces (IProvider) and extend Core base classes (BaseProvider)
- **VSCodeExtension/VisualStudioExtension** → **Api**: Extensions consume API endpoints
- **InfrastructureAsCode**: Defines Azure resources that host the solution components
- **AI** → **Core**: May reference Core domain models and interfaces

## Summary Table

| Project                | Role                                 | Depends On                         |
|------------------------|--------------------------------------|-----------------------------------|
| Api                    | REST API endpoints                   | Core, AI                          |
| Core                   | Business logic, models, interfaces   | None (base layer)                 |
| Infrastructure         | Provider factory and orchestration   | Core, Provider implementations    |
| AzureProvider          | Azure-specific implementations       | Core                              |
| GitHubProvider         | GitHub-specific implementations      | Core                              |
| AzureDevOpsProvider    | Azure DevOps implementations         | Core                              |
| AI                     | AI services and capabilities         | Core (optional)                   |
| VSCodeExtension        | VS Code extension                    | Api                               |
| VisualStudioExtension  | Visual Studio extension              | Api                               |
| InfrastructureAsCode   | Bicep templates for Azure resources  | None (deployment artifacts)       |

## Deployment Architecture

The InfrastructureAsCode project contains Bicep templates that define the Azure resources required to host the PETools solution:

- **App Service and App Service Plan**: Hosts the Api project
- **Application Insights & Log Analytics**: Provides monitoring and logging capabilities
- **Key Vault**: Securely stores configuration secrets for the application
- **Autoscale Settings**: Configures scaling rules based on CPU utilization

The deployment follows Azure best practices including:
- Managed identities for secure service-to-service communication
- Diagnostic settings for comprehensive logging
- Role-based access control for secure resource access

## Notes

- **Provider Pattern**: The solution uses a provider pattern where Core defines interfaces (IProvider) and base classes (BaseProvider), while specific provider implementations (Azure, GitHub, Azure DevOps) implement these interfaces.
- **Factory Pattern**: Infrastructure's ProviderFactory manages the creation of appropriate provider instances based on context.
- **Extensibility**: New providers can be added by implementing the Core interfaces and registering with the factory, without changing existing code.
- **Client Integration**: Extensions interact with the platform through the Api, ensuring loose coupling and service evolution.
- **Infrastructure as Code**: All Azure resources are defined in Bicep templates, enabling automated, consistent deployments.

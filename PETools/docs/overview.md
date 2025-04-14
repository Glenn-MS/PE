# Overview

The Platform Engineering Tools (PETools) documentation provides comprehensive guidance on how to install, configure, and use the PETools solution built on .NET 10.

## Key Features

- Modular and extensible architecture
- Infrastructure as Code using Azure Verified Modules
- Support for multiple cloud providers
- Integrated developer tools
- AI-powered assistance

## About PETools

PETools is a comprehensive Platform Engineering solution built on .NET 10 that helps teams streamline their development, deployment, and management processes. It includes:

1. **Core Services** - Foundational components for platform operations
2. **Infrastructure Management** - Tools for deploying and managing cloud resources
3. **Developer Extensions** - VS Code and Visual Studio integrations
4. **Cloud Provider Integrations** - Azure, GitHub, and Azure DevOps connectors

## Documentation Structure

- **[Getting Started](./getting-started/README.md)** - Installation and initial setup
- **[Architecture](./architecture/README.md)** - Architectural overview of the PETools platform
- **[Infrastructure](./infrastructure/README.md)** - Infrastructure as Code documentation using Azure Verified Modules (AVM)
- **[AI Layer](./ai/README.md)** - AI capabilities and integration
- **[API Layer](./api/README.md)** - RESTful API documentation
- **[Core Layer](./core/README.md)** - Foundational components and services
- **[Providers](./providers/README.md)** - Cloud provider integrations
- **[Client Extensions](./client-extensions/README.md)** - IDE integrations for developers

## Repository Structure

This repository is organized into several projects following .NET 10 clean architecture principles:

- **Core** - Contains core business logic, domain models, interfaces, and base abstractions
- **Infrastructure** - Provider factory for creating and managing service providers  
- **Provider Projects**:
  - **AzureProvider** - Azure-specific functionality and integration
  - **AzureDevOpsProvider** - Azure DevOps-specific functionality and integration
  - **GitHubProvider** - GitHub-specific functionality and integration
- **Api** - RESTful API endpoints exposed through controllers
- **AI** - AI services for intelligent platform features
- **Extensions**:
  - **VSCodeExtension** - Visual Studio Code extension for developer productivity
  - **VisualStudioExtension** - Visual Studio extension for developer productivity
- **InfrastructureAsCode** - Bicep templates for Azure resource deployment

## WAF and Azure Best Practices

The PETools platform aligns with the Microsoft Well-Architected Framework (WAF) and Azure Best Practices:

1. **Reliability**:
   - Redundant infrastructure components
   - Automated failover mechanisms

2. **Security**:
   - Role-based access control (RBAC)
   - Secure storage of secrets using Azure Key Vault

3. **Performance Efficiency**:
   - Optimized resource allocation
   - Scalable infrastructure

4. **Operational Excellence**:
   - Centralized logging and monitoring
   - Automated deployment pipelines

5. **Cost Optimization**:
   - Use of reserved instances and auto-scaling
   - Monitoring and alerting for cost anomalies

# Provider Framework Documentation

This document provides information about the Provider Framework used in PETools, focusing on the implementation and integration of various cloud service providers.

## Overview

The Provider Framework is a key architectural component of the PETools platform that enables modular integration with different cloud services and external systems. Each provider is implemented as a separate project that implements the interfaces defined in the Core project, following .NET 10 clean architecture principles.

## Provider Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                         Core Layer                              │
│  ┌──────────────┐ ┌───────────────────┐ ┌────────────────────┐  │
│  │  IProvider   │ │   BaseProvider    │ │   Domain Models    │  │
│  │  Interface   │ │  Abstract Class   │ │   & Interfaces     │  │
│  └──────────────┘ └───────────────────┘ └────────────────────┘  │
└────────────────────────────┬────────────────────────────────────┘
                            │
┌────────────────────────────┼────────────────────────────────────┐
│                 Infrastructure Layer                            │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                   Provider Factory                      │    │
│  └─────────────────────────────────────────────────────────┘    │
└────────────────────────────┬────────────────────────────────────┘
                            │
┌────────────────────────────┼────────────────────────────────────┐
│                    Provider Projects                             │
│  ┌────────────┐ ┌────────────────┐ ┌────────────┐               │
│  │  Azure     │ │ Azure DevOps   │ │  GitHub    │               │
│  │  Provider  │ │   Provider     │ │  Provider  │               │
│  └────────────┘ └────────────────┘ └────────────┘               │
└─────────────────────────────────────────────────────────────────┘
```

## Core Components

### IProvider Interface

The `IProvider` interface defines the contract that all providers must implement:

```csharp
public interface IProvider
{
    string Name { get; }
    string Version { get; }
    
    Task<bool> Initialize();
    Task<object> ExecuteOperation(string operationName, object parameters);
    bool SupportsOperation(string operationName);
    IEnumerable<string> GetSupportedOperations();
}
```

### BaseProvider Abstract Class

The `BaseProvider` abstract class provides common functionality for all providers:

```csharp
public abstract class BaseProvider : IProvider
{
    public abstract string Name { get; }
    public abstract string Version { get; }
    
    protected readonly ILogger _logger;
    
    protected BaseProvider(ILogger logger)
    {
        _logger = logger;
    }
    
    public abstract Task<bool> Initialize();
    public abstract Task<object> ExecuteOperation(string operationName, object parameters);
    public abstract bool SupportsOperation(string operationName);
    public abstract IEnumerable<string> GetSupportedOperations();
}
```

## Available Providers

### Azure Provider

The Azure Provider enables interaction with Azure services and resources, including:

- Resource management (create, update, delete)
- Subscription and tenant operations
- Infrastructure deployment through ARM templates and Bicep
- Azure KeyVault integration for secrets management

**Usage Example:**

```csharp
// Get an instance of the Azure provider
var azureProvider = providerFactory.GetProvider("Azure");

// Execute operations
var resourceGroups = await azureProvider.ExecuteOperation("ListResourceGroups", 
    new { SubscriptionId = "subscription-id" });

var deployResult = await azureProvider.ExecuteOperation("DeployTemplate", 
    new { 
        SubscriptionId = "subscription-id",
        ResourceGroupName = "rg-name",
        TemplatePath = "./templates/main.bicep",
        Parameters = new Dictionary<string, object>()
        {
            { "environment", "dev" },
            { "location", "westus2" }
        }
    });
```

### Azure DevOps Provider

The Azure DevOps Provider facilitates interaction with Azure DevOps services:

- Repository management
- Pipeline operations
- Work item tracking
- Project and team management

**Usage Example:**

```csharp
// Get an instance of the Azure DevOps provider
var azureDevOpsProvider = providerFactory.GetProvider("AzureDevOps");

// Execute operations
var repositories = await azureDevOpsProvider.ExecuteOperation("ListRepositories", 
    new { ProjectName = "MyProject" });

var pipeline = await azureDevOpsProvider.ExecuteOperation("GetPipeline", 
    new { ProjectName = "MyProject", PipelineId = 123 });
```

### GitHub Provider

The GitHub Provider enables interaction with GitHub services:

- Repository operations
- Issue and pull request management
- GitHub Actions workflows
- User and organization management

**Usage Example:**

```csharp
// Get an instance of the GitHub provider
var githubProvider = providerFactory.GetProvider("GitHub");

// Execute operations
var repositories = await githubProvider.ExecuteOperation("ListRepositories", 
    new { Owner = "myorg" });

var pullRequests = await githubProvider.ExecuteOperation("ListPullRequests", 
    new { Owner = "myorg", Repository = "myrepo" });
```

## Provider Factory

The Provider Factory is responsible for dynamically loading and instantiating providers based on configuration or runtime requirements:

```csharp
public class ProviderFactory
{
    private readonly Dictionary<string, IProvider> _providers = new();
    private readonly ILogger<ProviderFactory> _logger;

    public ProviderFactory(ILogger<ProviderFactory> logger)
    {
        _logger = logger;
        LoadProviders();
    }

    public IProvider GetProvider(string providerName)
    {
        if (_providers.TryGetValue(providerName, out var provider))
        {
            return provider;
        }
        
        throw new ProviderNotFoundException($"Provider {providerName} not found");
    }

    public IEnumerable<IProvider> GetAllProviders() => _providers.Values;

    private void LoadProviders()
    {
        // Dynamic loading of providers from assemblies
        // ...
    }
}
```

## Extending with Custom Providers

To create a custom provider, implement the `IProvider` interface or extend the `BaseProvider` class:

1. Create a new class library project
2. Reference the Core project
3. Implement the IProvider interface or extend BaseProvider
4. Build and place the assembly in the designated providers directory

**Example Custom Provider:**

```csharp
public class CustomProvider : BaseProvider
{
    public override string Name => "Custom";
    public override string Version => "1.0.0";

    public CustomProvider(ILogger logger) : base(logger)
    {
    }

    public override async Task<bool> Initialize()
    {
        _logger.LogInformation("Initializing Custom Provider");
        return true;
    }

    public override async Task<object> ExecuteOperation(string operationName, object parameters)
    {
        return operationName switch
        {
            "SampleOperation" => HandleSampleOperation(parameters),
            _ => throw new NotSupportedException($"Operation {operationName} is not supported")
        };
    }

    public override bool SupportsOperation(string operationName)
    {
        return new[] { "SampleOperation" }.Contains(operationName);
    }

    public override IEnumerable<string> GetSupportedOperations()
    {
        return new[] { "SampleOperation" };
    }

    private object HandleSampleOperation(object parameters)
    {
        _logger.LogInformation("Executing Sample Operation");
        // Implementation
        return new { Success = true };
    }
}
```

## Best Practices

1. **Error Handling**: Implement robust error handling in provider operations
2. **Logging**: Use the provided ILogger to log important events and errors
3. **Configuration**: Store provider configuration in a secure manner
4. **Authentication**: Use appropriate authentication mechanisms for external services
5. **Caching**: Implement caching for frequently accessed data
6. **Rate Limiting**: Respect API rate limits when interacting with external services
7. **Extensibility**: Design provider operations to be extensible

## Troubleshooting

### Common Issues

1. **Provider Not Found**: Ensure the provider assembly is in the correct location
2. **Authentication Failures**: Check credentials and token expiration
3. **Operation Not Supported**: Verify the operation name and parameters
4. **Connection Issues**: Check network connectivity to external services

# Infrastructure Components

This document provides detailed descriptions of the components that make up the Infrastructure layer in the PETools platform.

## Key Components

### 1. Provider Factory

The Provider Factory is responsible for dynamically loading and managing providers. It ensures that the correct provider is used for each operation.

```csharp
public class ProviderFactory
{
    private readonly Dictionary<string, IProvider> _providers = new();

    public IProvider GetProvider(string providerName)
    {
        if (_providers.TryGetValue(providerName, out var provider))
        {
            return provider;
        }
        throw new ProviderNotFoundException($"Provider {providerName} not found");
    }

    public void RegisterProvider(IProvider provider)
    {
        _providers[provider.Name] = provider;
    }
}
```

### 2. Azure Provider

The Azure Provider enables interaction with Azure services, including resource management and deployment.

### 3. Azure DevOps Provider

The Azure DevOps Provider facilitates operations related to pipelines, repositories, and work items.

### 4. GitHub Provider

The GitHub Provider provides functionality for managing repositories, issues, and pull requests.

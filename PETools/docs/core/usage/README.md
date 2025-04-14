# Core Usage Guide

This document provides a usage guide for the Core layer in the PETools platform, detailing how to interact with its components programmatically.

## Overview

The Core layer provides foundational components and services that are used across the PETools platform. These include interfaces, models, and utilities.

## Example Usage

### 1. Using the `IProvider` Interface

The `IProvider` interface defines the contract for all providers. Here's an example of how to use it:

```csharp
var provider = providerFactory.GetProvider("Azure");
if (provider.SupportsOperation("ListResourceGroups"))
{
    var result = await provider.ExecuteOperation("ListResourceGroups", new {
        SubscriptionId = "your-subscription-id"
    });
    Console.WriteLine(result);
}
```

### 2. Working with Models

The Core layer includes models like `ProviderMetadata` and `OperationResult`. Here's an example:

```csharp
var metadata = new ProviderMetadata
{
    Name = "Azure",
    Version = "1.0.0",
    Description = "Azure provider for managing resources."
};

Console.WriteLine($"Provider: {metadata.Name}, Version: {metadata.Version}");
```

### 3. Using Utilities

The Core layer provides utility classes for common tasks. For example, using a logging utility:

```csharp
ILogger logger = new ConsoleLogger();
logger.LogInformation("This is an informational message.");
```

### 4. Dependency Injection

The Core layer supports dependency injection for better testability and maintainability. Here's an example:

```csharp
services.AddSingleton<IProviderFactory, ProviderFactory>();
services.AddTransient<IAIService, AIService>();
```

# Infrastructure Usage Guide

This document provides a usage guide for the Infrastructure layer in the PETools platform, detailing how to interact with its components programmatically.

## Overview

The Infrastructure layer provides tools and services for managing cloud resources and integrating with providers like Azure, Azure DevOps, and GitHub.

## Example Usage

### 1. Using the Provider Factory

The Provider Factory dynamically loads and manages providers. Here's an example:

```csharp
var providerFactory = new ProviderFactory();
var azureProvider = providerFactory.GetProvider("Azure");

var resourceGroups = await azureProvider.ExecuteOperation("ListResourceGroups", new {
    SubscriptionId = "your-subscription-id"
});

Console.WriteLine(resourceGroups);
```

### 2. Deploying Infrastructure

The Infrastructure layer supports deploying resources using templates. Here's an example:

```csharp
var deploymentResult = await azureProvider.ExecuteOperation("DeployTemplate", new {
    TemplatePath = "./templates/main.bicep",
    Parameters = new {
        environment = "dev",
        location = "westus2"
    }
});

Console.WriteLine(deploymentResult);
```

### 3. Managing Repositories

The GitHub Provider allows you to manage repositories. Here's an example:

```csharp
var githubProvider = providerFactory.GetProvider("GitHub");
var repositories = await githubProvider.ExecuteOperation("ListRepositories", new {
    Owner = "your-organization"
});

Console.WriteLine(repositories);
```

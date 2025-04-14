# Platform Engineering Tools - Infrastructure as Code

This repository contains the Infrastructure as Code (IaC) implementation for the PETools platform, built on .NET 10 and following Azure Verified Modules (AVM) principles.

## Overview

The InfrastructureAsCode project provides a modular, reusable approach to deploying Azure resources for the Platform Engineering solution. The implementation follows Azure best practices and Well-Architected Framework principles, with a focus on security, reliability, and maintainability.

## Architecture

The IaC implementation is structured as follows:

```
InfrastructureAsCode/
├── main.bicep                 # Main orchestration template
├── main.parameters.json       # Parameters file
└── modules/                   # Modular components
    ├── app-service/           # App Service modules
    │   └── app-service.bicep  # App Service and Plan deployment
    ├── key-vault/             # Key Vault modules
    │   ├── key-vault.bicep    # Key Vault deployment
    │   └── role-assignment.bicep # RBAC assignments
    └── monitoring/            # Monitoring modules
        ├── monitoring.bicep   # Application Insights and Log Analytics
        └── diagnostic-settings.bicep # Resource diagnostic settings
```

## Prerequisites

- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) (version 2.40.0 or higher)
- [Bicep CLI](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/install) (version 0.13.1 or higher)
- Azure subscription with appropriate permissions
- PowerShell 7+ or Bash for deployment scripts

## Installation

### 1. Clone the Repository

```powershell
git clone https://github.com/your-organization/PETools.git
cd PETools
```

### 2. Login to Azure

```powershell
az login
az account set --subscription "Your Subscription Name"
```

### 3. Create a Resource Group (if needed)

```powershell
$environment = "dev"
$location = "westus2"
$resourceGroupName = "rg-${environment}"

az group create --name $resourceGroupName --location $location
```

## Usage

### Basic Deployment

To deploy the full infrastructure:

```powershell
# Set parameters
$environment = "dev"
$location = "westus2"

# Deploy the infrastructure
az deployment group create \
  --resource-group "rg-${environment}" \
  --template-file ./InfrastructureAsCode/main.bicep \
  --parameters environmentName=$environment location=$location
```

### Using a Parameters File

```powershell
az deployment group create \
  --resource-group "rg-${environment}" \
  --template-file ./InfrastructureAsCode/main.bicep \
  --parameters ./InfrastructureAsCode/main.parameters.json
```

### CI/CD Pipeline Deployment

You can integrate this IaC solution into your CI/CD pipelines (GitHub Actions, Azure DevOps) using the following steps:

1. Add service principal authentication
2. Use the deployment commands in your pipeline script
3. Pass parameters from pipeline variables

Example GitHub Actions workflow:

```yaml
name: Deploy Infrastructure

on:
  push:
    branches: [ main ]
    paths:
      - 'InfrastructureAsCode/**'

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v2
    - name: Azure Login
      uses: azure/login@v1
      with:
        creds: ${{ secrets.AZURE_CREDENTIALS }}
    
    - name: Deploy Bicep template
      uses: azure/arm-deploy@v1
      with:
        resourceGroupName: ${{ env.RESOURCE_GROUP }}
        template: ./InfrastructureAsCode/main.bicep
        parameters: environmentName=${{ env.ENVIRONMENT }} location=${{ env.LOCATION }}
```

## Use Cases

### 1. Development Environment

Deploy a development environment with minimal resources:

```powershell
az deployment group create \
  --resource-group "rg-dev" \
  --template-file ./InfrastructureAsCode/main.bicep \
  --parameters environmentName=dev location=westus2 \
  --parameters ./InfrastructureAsCode/environments/dev.parameters.json
```

### 2. Production Environment

Deploy a production environment with enhanced security and scaling:

```powershell
az deployment group create \
  --resource-group "rg-prod" \
  --template-file ./InfrastructureAsCode/main.bicep \
  --parameters environmentName=prod location=eastus2 \
  --parameters ./InfrastructureAsCode/environments/prod.parameters.json
```

### 3. Deploying Individual Modules

You can deploy individual modules for targeted scenarios:

```powershell
# Deploy only the App Service module
az deployment group create \
  --resource-group "rg-dev" \
  --template-file ./InfrastructureAsCode/modules/app-service/app-service.bicep \
  --parameters name=myapp-dev appServicePlanName=myapp-plan-dev location=westus2
```

## Module Details

### App Service Module

This module deploys an App Service with App Service Plan with these features:
- Secure settings (HTTPS only, TLS 1.2+)
- CORS configuration
- Identity management
- Scalable App Service Plan

**Example Usage:**
```bicep
module appService 'modules/app-service/app-service.bicep' = {
  name: 'app-service-deployment'
  params: {
    name: 'myapp-dev'
    appServicePlanName: 'myapp-plan-dev'
    location: location
    tags: {
      environment: 'development'
    }
    skuName: 'P1v2'
  }
}
```

### Key Vault Module

This module deploys a Key Vault with:
- RBAC authorization
- Network security
- Soft delete configuration
- Secret management

**Example Usage:**
```bicep
module keyVault 'modules/key-vault/key-vault.bicep' = {
  name: 'key-vault-deployment'
  params: {
    name: 'kv-myapp-dev'
    location: location
    enableRbacAuthorization: true
    networkDefaultAction: 'Deny'
  }
}
```

### Monitoring Module

This module deploys Application Insights and Log Analytics Workspace:
- Connected monitoring components
- Configurable retention periods
- Diagnostic collection

**Example Usage:**
```bicep
module monitoring 'modules/monitoring/monitoring.bicep' = {
  name: 'monitoring-deployment'
  params: {
    appInsightsName: 'ai-myapp-dev'
    logAnalyticsWorkspaceName: 'law-myapp-dev'
    location: location
    retentionInDays: 30
  }
}
```

## Best Practices

1. **Environment Segregation**: Use different parameter files for dev/test/prod
2. **Secret Management**: Never commit secrets to your repository
3. **Naming Conventions**: Follow consistent naming patterns
4. **Tagging Strategy**: Always tag resources for governance
5. **Least Privilege**: Use RBAC with minimum required permissions
6. **Monitoring**: Enable diagnostic settings for all resources

## Customization

You can customize these modules by:

1. Modifying module parameters in the main.bicep file
2. Extending module templates for additional resources
3. Creating new parameter files for different environments
4. Adding custom tags or naming conventions

## Integration with .NET Solution

This IaC project is part of a larger .NET 10 Platform Engineering solution. The `AzureProvider` project in the solution can programmatically interact with the deployed infrastructure.

Example integration with the .NET solution:

```csharp
// From AzureProvider project
public async Task<AppServiceDetails> GetAppServiceDetails(string environmentName)
{
    // Get the deployed app service details
    var appServiceName = $"{environmentName}-app";
    var appService = await _azureClient.GetAppServiceAsync(appServiceName);
    
    return new AppServiceDetails
    {
        Url = $"https://{appService.DefaultHostName}",
        ResourceId = appService.Id,
        State = appService.State
    };
}
```

## Troubleshooting

### Common Issues

1. **Deployment Failures**: Check the activity log in the Azure Portal
2. **Permission Issues**: Ensure your account has proper RBAC permissions
3. **Parameter Errors**: Validate parameter values match expected formats
4. **Resource Naming**: Ensure resource names follow Azure naming rules

### Getting Help

For issues with this infrastructure code:
1. Check the Azure Resource Manager documentation
2. Review the Bicep language specification
3. Submit an issue in the project repository

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request with detailed description

## License

This project is licensed under the MIT License - see the LICENSE file for details.

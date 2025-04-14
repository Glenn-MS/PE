# Infrastructure Use Cases

This document provides examples of common deployment scenarios using the Platform Engineering Tools Infrastructure as Code modules.

## Use Case Scenarios

1. [Development Environment Deployment](#development-environment-deployment)
2. [Production Environment Deployment](#production-environment-deployment)
3. [Individual Module Deployment](#individual-module-deployment)
4. [CI/CD Pipeline Integration](#cicd-pipeline-integration)

## Development Environment Deployment

This scenario demonstrates how to deploy a development environment with minimal resources.

### Deployment Script

```powershell
az deployment group create \
  --resource-group "rg-dev" \
  --template-file ./InfrastructureAsCode/main.bicep \
  --parameters environmentName=dev location=westus2 \
  --parameters ./InfrastructureAsCode/environments/dev.parameters.json
```

### Sample Parameters File (dev.parameters.json)

```json
{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentParameters.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "environmentName": {
      "value": "dev"
    },
    "location": {
      "value": "westus2"
    },
    "appServiceName": {
      "value": "petools-dev-app"
    },
    "appServicePlanName": {
      "value": "petools-dev-plan"
    },
    "appInsightsName": {
      "value": "petools-dev-ai"
    }
  }
}
```

## Production Environment Deployment

This scenario demonstrates how to deploy a production environment with enhanced security and scaling.

### Deployment Script

```powershell
az deployment group create \
  --resource-group "rg-prod" \
  --template-file ./InfrastructureAsCode/main.bicep \
  --parameters environmentName=prod location=eastus2 \
  --parameters ./InfrastructureAsCode/environments/prod.parameters.json
```

### Sample Parameters File (prod.parameters.json)

```json
{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentParameters.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "environmentName": {
      "value": "prod"
    },
    "location": {
      "value": "eastus2"
    },
    "appServiceName": {
      "value": "petools-prod-app"
    },
    "appServicePlanName": {
      "value": "petools-prod-plan"
    },
    "appInsightsName": {
      "value": "petools-prod-ai"
    }
  }
}
```

## Individual Module Deployment

This scenario demonstrates how to deploy individual modules for targeted scenarios.

### App Service Module Deployment

```powershell
az deployment group create \
  --resource-group "rg-dev" \
  --template-file ./InfrastructureAsCode/modules/app-service/app-service.bicep \
  --parameters name=myapp-dev appServicePlanName=myapp-plan-dev location=westus2
```

### Key Vault Module Deployment

```powershell
az deployment group create \
  --resource-group "rg-dev" \
  --template-file ./InfrastructureAsCode/modules/key-vault/key-vault.bicep \
  --parameters name=kv-myapp-dev location=westus2
```

### Monitoring Module Deployment

```powershell
az deployment group create \
  --resource-group "rg-dev" \
  --template-file ./InfrastructureAsCode/modules/monitoring/monitoring.bicep \
  --parameters appInsightsName=ai-myapp-dev logAnalyticsWorkspaceName=law-myapp-dev location=westus2
```

## CI/CD Pipeline Integration

This scenario demonstrates how to integrate the Infrastructure as Code with CI/CD pipelines.

### GitHub Actions Workflow

Create a file `.github/workflows/deploy-infrastructure.yml`:

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
    - uses: actions/checkout@v3
    
    - name: Set up environment variables
      run: |
        echo "ENVIRONMENT=dev" >> $GITHUB_ENV
        echo "LOCATION=westus2" >> $GITHUB_ENV
        echo "RESOURCE_GROUP=rg-dev" >> $GITHUB_ENV
    
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

### Azure DevOps Pipeline

Create a file `azure-pipelines.yml`:

```yaml
trigger:
  branches:
    include:
    - main
  paths:
    include:
    - InfrastructureAsCode/**

pool:
  vmImage: 'ubuntu-latest'

variables:
  environment: 'dev'
  location: 'westus2'
  resourceGroup: 'rg-dev'

steps:
- task: AzureCLI@2
  displayName: 'Deploy Infrastructure'
  inputs:
    azureSubscription: 'Your-Azure-Service-Connection'
    scriptType: 'bash'
    scriptLocation: 'inlineScript'
    inlineScript: |
      az deployment group create \
        --resource-group $(resourceGroup) \
        --template-file ./InfrastructureAsCode/main.bicep \
        --parameters environmentName=$(environment) location=$(location)
```

## Environment Promotion Strategy

This scenario demonstrates how to implement a promotion strategy for deploying to multiple environments.

### Deployment Pipeline

1. **Development**
   - Deploy to dev environment
   - Run automated tests
   - Validate infrastructure

2. **Staging**
   - Promote successful dev deployment to staging
   - Run integration tests
   - Validate against production-like data

3. **Production**
   - Promote successful staging deployment to production
   - Run smoke tests
   - Monitor for issues

### Sample Implementation Script

```powershell
# Deploy to Development
az deployment group create \
  --resource-group "rg-dev" \
  --template-file ./InfrastructureAsCode/main.bicep \
  --parameters ./InfrastructureAsCode/environments/dev.parameters.json

# Run tests (pseudo-code)
# Run-Tests -Environment "dev"

# If tests pass, deploy to Staging
az deployment group create \
  --resource-group "rg-staging" \
  --template-file ./InfrastructureAsCode/main.bicep \
  --parameters ./InfrastructureAsCode/environments/staging.parameters.json

# Run integration tests (pseudo-code)
# Run-IntegrationTests -Environment "staging"

# If tests pass, deploy to Production
az deployment group create \
  --resource-group "rg-prod" \
  --template-file ./InfrastructureAsCode/main.bicep \
  --parameters ./InfrastructureAsCode/environments/prod.parameters.json

# Run smoke tests (pseudo-code)
# Run-SmokeTests -Environment "prod"
```

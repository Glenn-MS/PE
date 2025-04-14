// PETools - Infrastructure as Code
// Main deployment file using Azure Verified Modules (AVM) patterns
// This modular approach improves maintainability and reusability

// Common parameters
@description('The name of the environment.')
param environmentName string

@description('The Azure location for the resources.')
param location string = resourceGroup().location

@description('The name of the resource group.')
param resourceGroupName string = 'rg-${environmentName}'

// Resource-specific parameters with defaults based on environment name
@description('The name of the App Service.')
param appServiceName string = '${environmentName}-app'

@description('The name of the App Service Plan.')
param appServicePlanName string = '${environmentName}-plan'

@description('The name of the Application Insights instance.')
param appInsightsName string = '${environmentName}-ai'

@description('The name of the Log Analytics workspace.')
param logAnalyticsName string = '${environmentName}-la'

@description('The name of the Key Vault.')
param keyVaultName string = '${environmentName}-kv-${resourceToken}'

// Variables
var resourceToken = uniqueString(environmentName, subscription().id)
var tags = {
  'azd-env-name': environmentName
  'environment': environmentName
  'project': 'PETools'
}

// Reference existing resource group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' existing = {
  name: resourceGroupName
}

// Deploy monitoring resources
module monitoring 'modules/monitoring/monitoring.bicep' = {
  name: 'monitoring-deployment'
  params: {
    appInsightsName: appInsightsName
    logAnalyticsWorkspaceName: logAnalyticsName
    location: location
    tags: tags
    retentionInDays: 30
  }
}

// Deploy App Service and App Service Plan
module appService 'modules/app-service/app-service.bicep' = {
  name: 'app-service-deployment'
  params: {
    name: appServiceName
    appServicePlanName: appServicePlanName
    location: location
    tags: tags
    skuName: 'P1v2'
    skuTier: 'PremiumV2'
    capacity: 1
    allowedOrigins: [
      'https://portal.azure.com'
      'https://${appServiceName}.azurewebsites.net'
    ]
    appSettings: [
      {
        name: 'WEBSITE_RUN_FROM_PACKAGE'
        value: '1'
      }
      {
        name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
        value: monitoring.outputs.appInsightsInstrumentationKey
      }
      {
        name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
        value: monitoring.outputs.appInsightsConnectionString
      }
      {
        name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
        value: '~3'
      }
    ]
  }
}

// Deploy Key Vault
module keyVault 'modules/key-vault/key-vault.bicep' = {
  name: 'key-vault-deployment'
  params: {
    name: keyVaultName
    location: location
    tags: tags
    networkDefaultAction: 'Deny'
    networkBypass: 'AzureServices'
  }
}

// Assign Key Vault Secrets User role to App Service
module keyVaultRoleAssignment 'modules/key-vault/role-assignment.bicep' = {
  name: 'key-vault-role-assignment'
  params: {
    principalId: appService.outputs.appServicePrincipalId
    resourceId: keyVault.outputs.keyVaultId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6') // Key Vault Secrets User
  }
  dependsOn: [
    keyVault
    appService
  ]
}

// Create a secret for Azure Web Jobs Storage
resource storageConnectionSecret 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: existingKeyVault
  name: 'AzureWebJobsStorage'
  properties: {
    value: 'DefaultEndpointsProtocol=https;AccountName=dummy;AccountKey=dummy;EndpointSuffix=core.windows.net'
  }
}

// Reference to deployed Key Vault for creating secrets
resource existingKeyVault 'Microsoft.KeyVault/vaults@2023-02-01' existing = {
  name: keyVaultName
}

// Configure App Service App Settings with Key Vault reference
resource appServiceAppSettings 'Microsoft.Web/sites/config@2024-04-01' = {
  name: 'appsettings'
  parent: existingAppService
  properties: {
    AzureWebJobsStorage: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=AzureWebJobsStorage)'
  }
  dependsOn: [
    storageConnectionSecret
    appService
  ]
}

// Reference to deployed App Service for configuring settings
resource existingAppService 'Microsoft.Web/sites@2024-04-01' existing = {
  name: appServiceName
}

// Configure diagnostic settings for App Service
module appServiceDiagnostics 'modules/monitoring/diagnostic-settings.bicep' = {
  name: 'app-service-diagnostics'
  params: {
    name: '${appServiceName}-diagnostics'
    resourceId: appService.outputs.appServiceId
    workspaceId: monitoring.outputs.logAnalyticsWorkspaceId
    logCategories: [
      { name: 'AppServiceHTTPLogs' }
      { name: 'AppServiceConsoleLogs' }
      { name: 'AppServiceAppLogs' }
      { name: 'AppServiceAuditLogs' }
    ]
    retentionDays: 30
  }
  dependsOn: [
    appService
    monitoring
  ]
}

// Configure auto-scaling for App Service Plan
resource appServiceAutoscaleSettings 'Microsoft.Insights/autoscalesettings@2022-10-01' = {
  name: '${appServiceName}-autoscale'
  location: location
  properties: {
    enabled: true
    targetResourceUri: appService.outputs.appServicePlanId
    profiles: [
      {
        name: 'Default'
        capacity: {
          minimum: '1'
          maximum: '3'
          default: '1'
        }
        rules: [
          {
            metricTrigger: {
              metricName: 'CpuPercentage'
              metricResourceUri: appService.outputs.appServicePlanId
              timeGrain: 'PT1M'
              statistic: 'Average'
              timeWindow: 'PT10M'
              timeAggregation: 'Average'
              operator: 'GreaterThan'
              threshold: 70
            }
            scaleAction: {
              direction: 'Increase'
              type: 'ChangeCount'
              value: '1'
              cooldown: 'PT10M'
            }
          }
          {
            metricTrigger: {
              metricName: 'CpuPercentage'
              metricResourceUri: appService.outputs.appServicePlanId
              timeGrain: 'PT1M'
              statistic: 'Average'
              timeWindow: 'PT10M'
              timeAggregation: 'Average'
              operator: 'LessThan'
              threshold: 30
            }
            scaleAction: {
              direction: 'Decrease'
              type: 'ChangeCount'
              value: '1'
              cooldown: 'PT10M'
            }
          }
        ]
      }
    ]
  }
}

// Outputs
output appServiceName string = appService.outputs.appServiceName
output appServicePlanId string = appService.outputs.appServicePlanId
output resourceGroupId string = resourceGroup.id
output appInsightsInstrumentationKey string = monitoring.outputs.appInsightsInstrumentationKey
output appInsightsConnectionString string = monitoring.outputs.appInsightsConnectionString
output keyVaultName string = keyVault.outputs.keyVaultName

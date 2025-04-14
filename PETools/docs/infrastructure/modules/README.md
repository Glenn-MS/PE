# Infrastructure Modules

This document provides details on the individual infrastructure modules that make up the Platform Engineering Tools' Infrastructure as Code implementation.

## Available Modules

The infrastructure is built using the following reusable modules:

1. [App Service Module](#app-service-module)
2. [Key Vault Module](#key-vault-module)
3. [Monitoring Module](#monitoring-module)
4. [Role Assignment Module](#role-assignment-module)
5. [Diagnostic Settings Module](#diagnostic-settings-module)

## App Service Module

This module deploys an App Service with App Service Plan with these features:
- Secure settings (HTTPS only, TLS 1.2+)
- CORS configuration
- Identity management
- Scalable App Service Plan

### Parameters

| Parameter | Type | Description | Default |
|-----------|------|-------------|---------|
| name | string | The name of the App Service | Required |
| appServicePlanName | string | The name of the App Service Plan | Required |
| location | string | The Azure location for the resources | Required |
| skuName | string | The SKU name for the App Service Plan | P1v2 |
| skuTier | string | The tier of the App Service Plan | PremiumV2 |
| capacity | int | The capacity of the App Service Plan | 1 |
| tags | object | Tags to apply to the resources | {} |
| allowedOrigins | array | The list of allowed CORS origins | [] |
| minTlsVersion | string | The minimum TLS version required | 1.2 |
| ftpsState | string | Whether FTPS is disabled | Disabled |
| appSettings | array | App settings for the App Service | [] |
| identityType | string | The identity type of the App Service | SystemAssigned |

### Outputs

- appServiceId: The resource ID of the deployed App Service
- appServiceName: The name of the deployed App Service
- appServicePlanId: The resource ID of the App Service Plan
- appServicePrincipalId: The principal ID of the App Service managed identity

### Example Usage

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
    allowedOrigins: [
      'https://portal.azure.com',
      'https://myapp-dev.azurewebsites.net'
    ]
    appSettings: [
      {
        name: 'WEBSITE_RUN_FROM_PACKAGE'
        value: '1'
      }
    ]
  }
}
```

## Key Vault Module

This module deploys a Key Vault with:
- RBAC authorization
- Network security
- Soft delete configuration
- Secret management

### Parameters

| Parameter | Type | Description | Default |
|-----------|------|-------------|---------|
| name | string | The name of the Key Vault | Required |
| location | string | The Azure location for the Key Vault | Required |
| tenantId | string | The tenant ID for the Key Vault | subscription().tenantId |
| tags | object | Tags to apply to the Key Vault | {} |
| enableRbacAuthorization | bool | Whether to enable RBAC authorization | true |
| enableSoftDelete | bool | Whether to enable soft delete | true |
| softDeleteRetentionInDays | int | The soft delete retention period in days | 90 |
| networkDefaultAction | string | The default action for network security | Deny |
| networkBypass | string | The bypass settings for network security | AzureServices |
| ipRules | array | Optional IP rules for network security | [] |
| virtualNetworkRules | array | Optional virtual network rules for network security | [] |

### Outputs

- keyVaultId: The resource ID of the deployed Key Vault
- keyVaultName: The name of the deployed Key Vault
- keyVaultUri: The URI of the deployed Key Vault

### Example Usage

```bicep
module keyVault 'modules/key-vault/key-vault.bicep' = {
  name: 'key-vault-deployment'
  params: {
    name: 'kv-myapp-dev'
    location: location
    tags: {
      environment: 'development'
    }
    enableRbacAuthorization: true
    networkDefaultAction: 'Deny'
    networkBypass: 'AzureServices'
  }
}
```

## Monitoring Module

This module deploys Application Insights and Log Analytics Workspace:
- Connected monitoring components
- Configurable retention periods
- Diagnostic collection

### Parameters

| Parameter | Type | Description | Default |
|-----------|------|-------------|---------|
| appInsightsName | string | The name of the Application Insights instance | Required |
| logAnalyticsWorkspaceName | string | The name of the Log Analytics Workspace | Required |
| location | string | The Azure location for the resources | Required |
| applicationType | string | The application type for Application Insights | web |
| tags | object | Tags to apply to the resources | {} |
| retentionInDays | int | The retention period in days for the Log Analytics Workspace | 30 |
| logAnalyticsSku | string | The SKU name for the Log Analytics Workspace | PerGB2018 |
| publicNetworkAccessForIngestion | string | Whether to enable public network access for ingestion | Enabled |
| publicNetworkAccessForQuery | string | Whether to enable public network access for query | Enabled |

### Outputs

- appInsightsId: The resource ID of the deployed Application Insights
- appInsightsName: The name of the deployed Application Insights
- appInsightsInstrumentationKey: The instrumentation key of the deployed Application Insights
- appInsightsConnectionString: The connection string of the deployed Application Insights
- logAnalyticsWorkspaceId: The resource ID of the deployed Log Analytics Workspace
- logAnalyticsWorkspaceName: The name of the deployed Log Analytics Workspace

### Example Usage

```bicep
module monitoring 'modules/monitoring/monitoring.bicep' = {
  name: 'monitoring-deployment'
  params: {
    appInsightsName: 'ai-myapp-dev'
    logAnalyticsWorkspaceName: 'law-myapp-dev'
    location: location
    tags: {
      environment: 'development'
    }
    retentionInDays: 30
  }
}
```

## Role Assignment Module

This module handles RBAC role assignments for Azure resources.

### Parameters

| Parameter | Type | Description | Default |
|-----------|------|-------------|---------|
| principalId | string | The principal ID to assign the role to | Required |
| resourceId | string | The resource ID of the resource to assign the role on | Required |
| roleDefinitionId | string | The role definition ID to assign | Required |
| principalType | string | The principal type of the assigned principal ID | ServicePrincipal |
| name | string | Optional name for the role assignment | Auto-generated GUID |

### Outputs

- roleAssignmentId: The resource ID of the created role assignment
- roleAssignmentName: The name of the created role assignment

### Example Usage

```bicep
module keyVaultRoleAssignment 'modules/key-vault/role-assignment.bicep' = {
  name: 'key-vault-role-assignment'
  params: {
    principalId: appService.outputs.appServicePrincipalId
    resourceId: keyVault.outputs.keyVaultId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6') // Key Vault Secrets User
  }
}
```

## Diagnostic Settings Module

This module deploys diagnostic settings for Azure resources.

### Parameters

| Parameter | Type | Description | Default |
|-----------|------|-------------|---------|
| name | string | The name of the diagnostic settings | Required |
| resourceId | string | The resource ID of the resource to apply diagnostic settings to | Required |
| workspaceId | string | The resource ID of the Log Analytics workspace to send logs to | Required |
| logCategories | array | The log categories to enable | [] |
| enableMetrics | bool | Whether to enable metrics | true |
| retentionDays | int | The retention period in days for logs and metrics | 30 |

### Outputs

- diagnosticSettingsId: The resource ID of the created diagnostic settings
- diagnosticSettingsName: The name of the created diagnostic settings

### Example Usage

```bicep
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
}
```

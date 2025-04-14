// Diagnostic Settings Module following AVM patterns
// This module deploys diagnostic settings for Azure resources

@description('The name of the diagnostic settings.')
param name string

@description('The resource ID of the resource to apply diagnostic settings to.')
param resourceId string

@description('The resource ID of the Log Analytics workspace to send logs to.')
param workspaceId string

@description('The log categories to enable.')
param logCategories array = []

@description('Whether to enable metrics.')
param enableMetrics bool = true

@description('The retention period in days for logs and metrics.')
@minValue(0)
@maxValue(365)
param retentionDays int = 30

// Diagnostic Settings resource
resource diagnosticSettings 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: name
  scope: resourceRefObject
  properties: {
    workspaceId: workspaceId
    logs: [for category in logCategories: {
      category: category.name
      enabled: true
      retentionPolicy: {
        days: retentionDays
        enabled: retentionDays > 0
      }
    }]
    metrics: enableMetrics ? [
      {
        category: 'AllMetrics'
        enabled: true
        retentionPolicy: {
          days: retentionDays
          enabled: retentionDays > 0
        }
      }
    ] : []
  }
}

// This is a reference to the resource using an 'existing' keyword
// Since the resource ID is dynamic, we need to use the 'scope' syntax
resource resourceRefObject 'Microsoft.Resources/deployments@2022-09-01' existing = {
  name: split(resourceId, '/')[length(split(resourceId, '/')) - 1]
  scope: resourceGroup()
}

// Outputs
output diagnosticSettingsId string = diagnosticSettings.id
output diagnosticSettingsName string = diagnosticSettings.name

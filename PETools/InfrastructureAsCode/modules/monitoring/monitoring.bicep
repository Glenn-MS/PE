// Monitoring Module following AVM patterns
// This module deploys Application Insights and Log Analytics Workspace

@description('The name of the Application Insights instance.')
param appInsightsName string

@description('The name of the Log Analytics Workspace.')
param logAnalyticsWorkspaceName string

@description('The Azure location where the resources will be deployed.')
param location string

@description('The application type for Application Insights.')
@allowed(['web', 'other'])
param applicationType string = 'web'

@description('Tags to apply to the resources.')
param tags object = {}

@description('The retention period in days for the Log Analytics Workspace.')
@minValue(30)
@maxValue(730)
param retentionInDays int = 30

@description('The SKU name for the Log Analytics Workspace.')
param logAnalyticsSku string = 'PerGB2018'

@description('Whether to enable public network access for ingestion.')
param publicNetworkAccessForIngestion string = 'Enabled'

@description('Whether to enable public network access for query.')
param publicNetworkAccessForQuery string = 'Enabled'

// Log Analytics Workspace resource
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: logAnalyticsWorkspaceName
  location: location
  properties: {
    sku: {
      name: logAnalyticsSku
    }
    retentionInDays: retentionInDays
    features: {
      enableLogAccessUsingOnlyResourcePermissions: true
    }
  }
  tags: tags
}

// Application Insights resource
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: applicationType
    WorkspaceResourceId: logAnalyticsWorkspace.id
    publicNetworkAccessForIngestion: publicNetworkAccessForIngestion
    publicNetworkAccessForQuery: publicNetworkAccessForQuery
  }
  tags: tags
}

// Outputs
output appInsightsId string = appInsights.id
output appInsightsName string = appInsights.name
output appInsightsInstrumentationKey string = appInsights.properties.InstrumentationKey
output appInsightsConnectionString string = appInsights.properties.ConnectionString
output logAnalyticsWorkspaceId string = logAnalyticsWorkspace.id
output logAnalyticsWorkspaceName string = logAnalyticsWorkspace.name

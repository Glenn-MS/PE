// App Service Module following AVM patterns
// This module deploys an App Service with App Service Plan

@description('The name of the App Service.')
param name string

@description('The name of the App Service Plan.')
param appServicePlanName string

@description('The Azure location where the resources will be deployed.')
param location string

@description('The SKU name for the App Service Plan.')
@allowed([
  'B1', 'B2', 'B3',
  'S1', 'S2', 'S3',
  'P1v2', 'P2v2', 'P3v2',
  'P1v3', 'P2v3', 'P3v3'
])
param skuName string = 'P1v2'

@description('The tier of the App Service Plan.')
param skuTier string = 'PremiumV2'

@description('The capacity of the App Service Plan.')
@minValue(1)
@maxValue(10)
param capacity int = 1

@description('Tags to apply to the resources.')
param tags object = {}

@description('The list of allowed CORS origins.')
param allowedOrigins array = []

@description('The minimum TLS version required.')
param minTlsVersion string = '1.2'

@description('Whether FTPS is disabled.')
param ftpsState string = 'Disabled'

@description('App settings for the App Service.')
param appSettings array = []

@description('The identity type of the App Service.')
@allowed(['None', 'SystemAssigned', 'UserAssigned'])
param identityType string = 'SystemAssigned'

// App Service Plan resource
resource appServicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: skuName
    tier: skuTier
    size: skuName
    capacity: capacity
  }
  tags: tags
}

// App Service resource
resource appService 'Microsoft.Web/sites@2024-04-01' = {
  name: name
  location: location
  identity: {
    type: identityType
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      cors: {
        allowedOrigins: allowedOrigins
      }
      minTlsVersion: minTlsVersion
      ftpsState: ftpsState
      appSettings: appSettings
    }
  }
  tags: tags
}

// Outputs
output appServiceId string = appService.id
output appServiceName string = appService.name
output appServicePlanId string = appServicePlan.id
output appServicePrincipalId string = appService.identity.principalId

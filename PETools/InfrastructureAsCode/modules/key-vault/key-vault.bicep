// Key Vault Module following AVM patterns
// This module deploys a Key Vault with secure settings and optional RBAC

@description('The name of the Key Vault.')
param name string

@description('The Azure location where the Key Vault will be deployed.')
param location string

@description('The tenant ID for the Key Vault.')
param tenantId string = subscription().tenantId

@description('Tags to apply to the Key Vault.')
param tags object = {}

@description('Whether to enable RBAC authorization.')
param enableRbacAuthorization bool = true

@description('Whether to enable soft delete.')
param enableSoftDelete bool = true

@description('The soft delete retention period in days.')
@minValue(7)
@maxValue(90)
param softDeleteRetentionInDays int = 90

@description('The default action for network security ("Allow" or "Deny").')
@allowed(['Allow', 'Deny'])
param networkDefaultAction string = 'Deny'

@description('The bypass settings for network security.')
@allowed(['None', 'AzureServices'])
param networkBypass string = 'AzureServices'

@description('Optional IP rules for network security.')
param ipRules array = []

@description('Optional virtual network rules for network security.')
param virtualNetworkRules array = []

// Key Vault resource
resource keyVault 'Microsoft.KeyVault/vaults@2023-02-01' = {
  name: name
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: tenantId
    enableRbacAuthorization: enableRbacAuthorization
    enableSoftDelete: enableSoftDelete
    softDeleteRetentionInDays: softDeleteRetentionInDays
  }
  tags: tags
}

// Network security for Key Vault
resource keyVaultNetworkAcls 'Microsoft.KeyVault/vaults/networkAcls@2023-02-01' = {
  parent: keyVault
  name: 'default'
  properties: {
    defaultAction: networkDefaultAction
    bypass: networkBypass
    ipRules: ipRules
    virtualNetworkRules: virtualNetworkRules
  }
}

// Outputs
output keyVaultId string = keyVault.id
output keyVaultName string = keyVault.name
output keyVaultUri string = keyVault.properties.vaultUri

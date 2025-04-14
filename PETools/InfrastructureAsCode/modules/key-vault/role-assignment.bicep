// Role Assignment Module following AVM patterns
// This module handles RBAC role assignments for Azure resources

@description('The principal ID to assign the role to.')
param principalId string

@description('The resource ID of the resource to assign the role on.')
param resourceId string

@description('The role definition ID to assign.')
param roleDefinitionId string

@description('The principal type of the assigned principal ID.')
@allowed(['Device', 'ForeignGroup', 'Group', 'ServicePrincipal', 'User'])
param principalType string = 'ServicePrincipal'

@description('Optional name for the role assignment. A GUID will be generated if not provided.')
param name string = guid(resourceId, principalId, roleDefinitionId)

// Role Assignment resource
resource roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: name
  scope: resourceExists ? existingResource : null
  properties: {
    roleDefinitionId: roleDefinitionId
    principalId: principalId
    principalType: principalType
  }
}

// This is a workaround to handle the case when the resource ID doesn't exist yet
// We do this by checking if the resource exists before creating the role assignment
resource existingResource 'Microsoft.Resources/deployments@2022-09-01' existing = {
  name: split(resourceId, '/')[length(split(resourceId, '/')) - 1]
  scope: resourceGroup()
}

var resourceExists = length(split(resourceId, '/')) > 3

// Outputs
output roleAssignmentId string = roleAssignment.id
output roleAssignmentName string = roleAssignment.name

# API Usage Guide

This document provides a usage guide for the API layer in the PETools platform, detailing how to interact with its endpoints programmatically and through external tools.

## Overview

The API layer exposes RESTful endpoints that allow client applications, extensions, and external systems to interact with the platform's capabilities.

## Example API Requests

### 1. AI Operations

**Endpoint**: `POST /api/ai/execute`

**Request**:
```http
POST /api/ai/execute
Content-Type: application/json

{
  "operationName": "GenerateCodeSnippet",
  "parameters": {
    "Language": "C#",
    "Description": "Create a method to calculate factorial"
  }
}
```

**Response**:
```json
{
  "success": true,
  "result": "public int Factorial(int n) { return n <= 1 ? 1 : n * Factorial(n - 1); }"
}
```

### 2. Infrastructure Deployment

**Endpoint**: `POST /api/infrastructure/deploy`

**Request**:
```http
POST /api/infrastructure/deploy
Content-Type: application/json

{
  "templatePath": "./templates/main.bicep",
  "parameters": {
    "environment": "dev",
    "location": "westus2"
  }
}
```

**Response**:
```json
{
  "success": true,
  "deploymentId": "12345"
}
```

### 3. Provider Operations

**Endpoint**: `POST /api/providers/{providerName}/execute`

**Request**:
```http
POST /api/providers/Azure/execute
Content-Type: application/json

{
  "operationName": "ListResourceGroups",
  "parameters": {
    "SubscriptionId": "your-subscription-id"
  }
}
```

**Response**:
```json
{
  "success": true,
  "resourceGroups": [
    { "name": "rg-dev", "location": "westus2" },
    { "name": "rg-prod", "location": "eastus2" }
  ]
}
```

## Tools for Testing

1. **Postman**:
   - Import the API collection and test endpoints interactively.

2. **curl**:
   - Use the command line to send HTTP requests.

3. **Swagger UI**:
   - Access the API documentation and test endpoints directly in the browser.

# Getting Started with Platform Engineering Tools

This guide will help you get started with the Platform Engineering Tools (PETools) platform.

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) (version 2.40.0 or higher)
- [Bicep CLI](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/install) (version 0.13.1 or higher)
- Azure subscription with appropriate permissions
- PowerShell 7+ or Bash for deployment scripts
- [Visual Studio 2024](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

## Installation

### 1. Clone the Repository

```powershell
git clone https://github.com/your-organization/PETools.git
cd PETools
```

### 2. Build the Solution

```powershell
dotnet restore
dotnet build
```

### 3. Login to Azure

```powershell
az login
az account set --subscription "Your Subscription Name"
```

### 4. Create a Resource Group (if needed)

```powershell
$environment = "dev"
$location = "westus2"
$resourceGroupName = "rg-${environment}"

az group create --name $resourceGroupName --location $location
```

## Configuration

### API Configuration

The API project requires configuration of the `appsettings.json` file. Create or modify the `appsettings.Development.json` file:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Azure": {
    "TenantId": "your-tenant-id",
    "SubscriptionId": "your-subscription-id"
  }
}
```

### Azure Provider Configuration

If you're using the Azure Provider, you'll need to ensure the appropriate authentication:

```csharp
// Example configuration in your application
builder.Services.AddAzureProvider(options =>
{
    options.TenantId = builder.Configuration["Azure:TenantId"];
    options.SubscriptionId = builder.Configuration["Azure:SubscriptionId"];
});
```

## Running the Application

### Running the API

```powershell
cd Api
dotnet run
```

The API will start at `https://localhost:7001` and `http://localhost:5001` by default.

## Next Steps

- Explore the [Architecture Documentation](../architecture/README.md) to understand the system structure
- Deploy infrastructure using the [Infrastructure as Code](../infrastructure/README.md) documentation
- Learn about the different [Module Implementations](../infrastructure/modules/README.md)

## Troubleshooting

### Common Issues

1. **Authentication Errors**: Ensure you're properly logged in to Azure with `az login`
2. **Missing Dependencies**: Make sure all prerequisites are installed
3. **Build Errors**: Check for missing NuGet packages with `dotnet restore`

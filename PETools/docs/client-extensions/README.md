# Client Extensions Documentation

This document provides information about the Client Extensions available in the PETools platform, focusing on integrations with popular development environments like Visual Studio Code and Visual Studio.

## Overview

The Client Extensions component of PETools provides seamless integration with development environments, making platform engineering capabilities directly accessible from within your IDE. These extensions enhance developer productivity by allowing access to PETools functionalities without leaving the development environment.

## Extension Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                      IDE Extensions                             │
│  ┌──────────────────────┐      ┌───────────────────────────┐    │
│  │  Visual Studio Code  │      │      Visual Studio        │    │
│  │      Extension       │      │        Extension          │    │
│  └──────────────────────┘      └───────────────────────────┘    │
└────────────────────────────┬────────────────────────────────────┘
                            │
┌────────────────────────────┼────────────────────────────────────┐
│                    Extension API Client                         │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │           Authentication & Communication Layer           │   │
│  └──────────────────────────────────────────────────────────┘   │
└────────────────────────────┬────────────────────────────────────┘
                            │
┌────────────────────────────┼────────────────────────────────────┐
│                        PETools API                              │
│  ┌────────────┐ ┌────────────────┐ ┌────────────────────────┐   │
│  │  Provider  │ │       AI       │ │     Infrastructure     │   │
│  │  Endpoints │ │    Endpoints   │ │       Endpoints        │   │
│  └────────────┘ └────────────────┘ └────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
```

## Available Extensions

### Visual Studio Code Extension

The Visual Studio Code extension integrates PETools capabilities directly into VS Code, providing:

- **Platform Operations Panel**: Quick access to common platform engineering tasks
- **AI-assisted Development**: Integration with the PETools AI capabilities
- **Infrastructure Management**: Deploy and manage infrastructure from within VS Code
- **Terminal Integration**: Run platform commands directly in the VS Code terminal
- **Status Bar Integration**: View platform status and quick actions
- **Provider Explorer**: Browse and interact with configured providers

#### Features

1. **Provider Explorer**
   - Browse Azure resources, Azure DevOps projects, and GitHub repositories
   - Execute operations like deployment, project creation, and repository management

2. **AI Assistant**
   - Ask questions about your platform
   - Get suggestions for infrastructure improvements
   - Generate code snippets and templates

3. **Infrastructure Management**
   - View, create, update, and delete infrastructure resources
   - Deploy templates and monitor deployment status
   - View resource dependencies and relationships

4. **Command Palette Integration**
   - Access PETools commands from the VS Code command palette
   - Run complex operations with simple commands

#### Installation

1. Install from VS Code Marketplace:
   - Search for "PETools" in the VS Code extension marketplace
   - Click "Install"

2. Manual Installation:
   - Download the VSIX package from the releases page
   - In VS Code, go to Extensions, click the "..." button, and select "Install from VSIX"
   - Select the downloaded VSIX file

#### Configuration

Configure the extension using the settings.json file:

```json
{
  "petools.api.url": "https://localhost:7001",
  "petools.api.authentication.method": "azureAd",
  "petools.providers.azure.subscription": "default",
  "petools.providers.azureDevOps.organization": "your-organization",
  "petools.providers.github.organization": "your-organization"
}
```

### Visual Studio Extension

The Visual Studio extension brings PETools capabilities into Visual Studio, providing:

- **Platform Tools Window**: Dedicated tool window for platform engineering tasks
- **Solution Explorer Integration**: Right-click menus for platform operations
- **AI-assisted Development**: Access to PETools AI capabilities from within Visual Studio
- **Template Integration**: Project and item templates for platform components

#### Features

1. **Platform Tools Window**
   - Dashboard showing platform health and recent activities
   - Provider explorer for browsing resources
   - Operations panel for common tasks

2. **Solution Integration**
   - Deploy projects directly from Solution Explorer
   - Generate infrastructure templates for your application
   - Verify configuration against infrastructure requirements

3. **AI Code Assistance**
   - Get suggestions for platform-related code
   - Generate infrastructure templates based on project requirements
   - Documentation lookup and contextual help

4. **Debugging Integration**
   - Attach to cloud resources for debugging
   - View logs and diagnostics from within Visual Studio

#### Installation

1. Install from Visual Studio Marketplace:
   - In Visual Studio, go to Extensions > Manage Extensions
   - Search for "PETools"
   - Click "Download" and restart Visual Studio

2. Manual Installation:
   - Download the VSIX package from the releases page
   - Double-click the VSIX file to install
   - Restart Visual Studio

#### Configuration

Configure the extension using the Tools > Options > PETools menu:

- **API Endpoint**: URL of the PETools API
- **Authentication Settings**: Azure AD, API Key, or other authentication methods
- **Provider Configuration**: Settings for each provider (Azure, Azure DevOps, GitHub)
- **AI Features**: Enable or disable AI-assisted features

## Extension Development

### Visual Studio Code Extension

The VS Code extension is built using the VS Code Extension API and TypeScript:

```
VSCodeExtension/
├── package.json            # Extension manifest
├── tsconfig.json           # TypeScript configuration
├── .vscodeignore           # Files to exclude from package
├── src/
│   ├── extension.ts        # Extension entry point
│   ├── providers/          # Provider integration
│   ├── views/              # WebView implementations
│   ├── commands/           # Command implementations
│   └── utils/              # Utility functions
└── resources/              # Images and other resources
```

To build the VS Code extension:

```bash
cd VSCodeExtension
npm install
npm run compile
```

### Visual Studio Extension

The Visual Studio extension is built using the Visual Studio SDK and C#:

```
VisualStudioExtension/
├── VisualStudioExtension.csproj  # Project file
├── source.extension.vsixmanifest # Extension manifest
├── VSCommandTable.vsct           # Command table
├── ToolWindows/                  # Tool window implementations
│   ├── PlatformToolWindowControl.xaml
│   └── PlatformToolWindowControl.xaml.cs
├── Commands/                     # Command implementations
└── Services/                     # Extension services
```

To build the Visual Studio extension:

```bash
cd VisualStudioExtension
dotnet build
```

## Integration with PETools API

Both extensions communicate with the PETools API to access platform capabilities:

```csharp
public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationProvider _authProvider;
    
    public ApiClient(string apiUrl, IAuthenticationProvider authProvider)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(apiUrl) };
        _authProvider = authProvider;
    }
    
    public async Task<T> GetAsync<T>(string endpoint)
    {
        var token = await _authProvider.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content);
    }
    
    // Other HTTP methods (POST, PUT, DELETE, etc.)
}
```

## Authentication Flow

The extensions support multiple authentication methods:

1. **Azure AD Authentication**:
   - Interactive login using browser
   - Device code flow for headless environments
   - Token caching and refresh

2. **API Key Authentication**:
   - Simple API key authentication for development scenarios
   - Key management through extension settings

3. **Personal Access Token**:
   - For Azure DevOps and GitHub integration

## Best Practices

1. **Error Handling**: Implement robust error handling for API communication
2. **Cache Management**: Cache API responses to reduce network calls
3. **Progressive Disclosure**: Show simple options first, with advanced options available when needed
4. **User Feedback**: Provide clear feedback for long-running operations
5. **Configuration Validation**: Validate settings and provide helpful error messages

## Troubleshooting

### Common Issues

1. **Authentication Errors**: Ensure credentials are correct and tokens are not expired
2. **Connection Issues**: Verify the API URL is correct and accessible
3. **Missing Features**: Check if the required provider is loaded and accessible
4. **Performance Issues**: Check for large response data or network latency

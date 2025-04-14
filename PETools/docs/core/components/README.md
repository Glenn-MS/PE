# Core Components

This document provides detailed descriptions of the components that make up the Core layer in the PETools platform.

## Key Components

### 1. Interfaces

The Core layer defines interfaces that standardize interactions between components. Key interfaces include:

- **`IProvider`**: Defines the contract for all providers.
  ```csharp
  public interface IProvider
  {
      string Name { get; }
      string Version { get; }
      Task<bool> Initialize();
      Task<object> ExecuteOperation(string operationName, object parameters);
      bool SupportsOperation(string operationName);
      IEnumerable<string> GetSupportedOperations();
  }
  ```

- **`IService`**: Defines the contract for core services.

### 2. Models

The Core layer includes domain models and entities used across the platform. Examples:

- **Provider Metadata**:
  ```csharp
  public class ProviderMetadata
  {
      public string Name { get; set; }
      public string Version { get; set; }
      public string Description { get; set; }
  }
  ```

- **Operation Result**:
  ```csharp
  public class OperationResult
  {
      public bool Success { get; set; }
      public object Data { get; set; }
      public string ErrorMessage { get; set; }
  }
  ```

### 3. Utilities

The Core layer provides utility classes and methods for common tasks, such as:

- **Logging**: Standardized logging across all layers.
- **Validation**: Input validation utilities.
- **Configuration**: Helpers for reading and managing configuration settings.

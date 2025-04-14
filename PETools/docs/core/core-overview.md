# Core Layer Documentation

The Core layer is the foundational component of the PETools platform, containing essential business logic, domain models, interfaces, and base abstractions. It serves as the central hub that connects all other components in the solution.

## Purpose

Core establishes the domain model and business logic for the entire platform while remaining independent of external concerns. This design follows .NET 10 clean architecture principles to ensure:

- **Separation of Concerns**: Clear boundaries between different types of functionality
- **Dependency Inversion**: Dependencies point inward toward the domain model
- **Testability**: Business logic can be tested independent of external dependencies
- **Maintainability**: Changes to external systems have minimal impact on core business logic

## Key Components

- **Domain Models**: Core business entities and value objects
- **Interfaces**: Provider contracts (IProvider) that define capabilities
- **Base Abstractions**: Base classes (BaseProvider) that provide shared functionality
- **Business Logic**: Core services that implement the platform's business rules

## Integration Points

- **Api**: Consumes Core services and models to expose functionality via REST
- **Infrastructure**: Implements Core interfaces for provider factory functionality
- **Provider Projects**: Implement Core interfaces for specific cloud platforms
- **AI**: May leverage Core models and interfaces for AI-enhanced platform features

For detailed information, refer to the following sections:

- **[Components](./components/README.md)**: Detailed descriptions of Core components.
- **[Usage](./usage/README.md)**: How to use the Core layer.
- **[Best Practices](./best-practices/README.md)**: Guidelines for Core development and maintenance.
- **[Troubleshooting](./troubleshooting/README.md)**: Solutions for common Core issues.

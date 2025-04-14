# Architecture Overview

This document provides an overview of the Platform Engineering Tools (PETools) architecture, which follows .NET 10 clean architecture principles with clear separation of concerns. For detailed information, refer to the following sections:

- **[Reference Architecture](./reference-architecture.md)**: The definitive reference for the PETools modular architecture.
- **[Design Principles](./design-principles/architecture-design-principles.md)**: Core principles guiding the architecture.
- **[Best Practices](./best-practices/architecture-best-practices.md)**: Guidelines for maintaining and evolving the architecture.
- **[System Components](./components/architecture-components.md)**: Detailed descriptions of architectural components.
- **[Troubleshooting](./troubleshooting/architecture-troubleshooting.md)**: Solutions for common architectural challenges.

## Key Architectural Patterns

PETools implements several architectural patterns to ensure maintainability, extensibility, and scalability:

1. **Provider Pattern**: Core defines interfaces and base abstractions, with specific implementations in provider projects
2. **Factory Pattern**: ProviderFactory dynamically creates appropriate provider instances based on context
3. **Clean Architecture**: Clear separation of concerns with domain models at the center
4. **Dependency Injection**: Services are registered and injected rather than directly instantiated
5. **Infrastructure as Code**: Azure resources defined in Bicep templates for consistent deployments

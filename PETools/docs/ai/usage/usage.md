# AI Layer - Usage Guide

This document provides a usage guide for the AI layer in the PETools platform, detailing how to interact with its capabilities programmatically and through the API.

## Overview

The AI layer provides advanced capabilities such as:

- **Agent-based Operations**: Execute tasks using AI agents with contextual understanding.
- **Code Generation**: Generate code snippets and templates.
- **Decision Support**: Assist in decision-making processes with AI-driven insights.
- **Natural Language Queries**: Interact with the platform using natural language.

## Programmatic Usage

### Initializing the AI Service

The AI service can be initialized and used programmatically in the Core or API layers:

```csharp
var aiService = new AIService();
await aiService.InitializeAsync();
```

### Executing AI Operations

Use the `ExecuteOperation` method to perform AI-driven tasks:

```csharp
var result = await aiService.ExecuteOperation("GenerateCodeSnippet", new {
    Language = "C#",
    Description = "Create a method to calculate factorial",
});

Console.WriteLine(result);
```

### Supported Operations

| Operation Name       | Description                                      |
|----------------------|--------------------------------------------------|
| `GenerateCodeSnippet`| Generates a code snippet based on a description |
| `AnswerQuestion`     | Provides an answer to a natural language query   |
| `SummarizeText`      | Summarizes a given text                          |
| `PlanTask`           | Creates a task plan based on input requirements  |

## API Usage

The AI layer is exposed through the API layer, allowing external systems to interact with its capabilities.

### Example API Request

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

### Example API Response

```json
{
  "success": true,
  "result": "public int Factorial(int n) { return n <= 1 ? 1 : n * Factorial(n - 1); }"
}
```

## Integration with Extensions

The AI layer is integrated into the Visual Studio Code and Visual Studio extensions, enabling:

- **Code Suggestions**: AI-driven code suggestions while typing.
- **Natural Language Queries**: Ask questions about your code or platform.
- **Task Automation**: Automate repetitive tasks using AI agents.

### Example: Using AI in VS Code

1. Open the command palette (`Ctrl+Shift+P`).
2. Search for `PETools: Ask AI`.
3. Enter your query (e.g., "Generate a C# method to calculate factorial").
4. View the result in the output panel.

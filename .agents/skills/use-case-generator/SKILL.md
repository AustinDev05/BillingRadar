---
name: use-case-generator
description: Generates a new CQRS use case (Command or Query) in BillingRadar.Application following project conventions, MediatR, and Result wrapper pattern. Triggers when asked to "crear un nuevo caso de uso", "agregar comando", "agregar query", "generar handler CQRS", or "crear use case".
---

# Use Case Generator Instructions

When generating a new use case in `BillingRadar.Application`, strictly follow the project's existing structure and naming conventions:

## 1. File & Directory Structure
Place all use case files under `BillingRadar.Application/Modules/{ModuleName}/[Command|Query]/`:
- `{UseCaseName}[Command|Query].cs` (Request)
- `{UseCaseName}[Command|Query]Handler.cs` (Handler)
- `{UseCaseName}[Command|Query]Response.cs` (Response DTO, if query or command with output)

## 2. Request Specification
- Must be defined as a C# `public record`.
- Must implement `IRequest<Result<TResponse>>` (or `IRequest<Result>` if no return value).
- Namespaces must follow: `BillingRadar.Application.Modules.{ModuleName}.[Command|Query]`.

## 3. Response DTO Specification
- MUST be defined as a C# `public record` with camelCase parameters in positional constructors (e.g. `public record ResponseDto(string propertyName, int count);`).
- **CRITICAL**: Do NOT expose Domain entities (`BillingRadar.Domain.Entities.*`) directly in the response.

## 4. Handler Implementation
- Must implement `IRequestHandler<{RequestName}, Result<{ResponseName}>>`.
- Inject interfaces from `BillingRadar.Domain.Repositories` or `BillingRadar.Application.Interfaces` via constructor.
- Use `Result<{ResponseName}>.Success(...)` and `Result<{ResponseName}>.Failure(...)` for domain/business outcome handling.

## 5. Reference Implementation
Refer to `.agents/skills/use-case-generator/examples/AuthQueryExample.md` for a real project reference implementation based on `Auth/Query`.

# BillingRadar Workspace Rules & Architecture Directives

Operating context: C# / .NET 10, Clean Architecture, MediatR (CQRS), Entity Framework Core (PostgreSQL).

## 1. Domain Entities & Invariants
- All entity properties in `BillingRadar.Domain.Entities` must use `private set` for encapsulation (as established in `User.cs`).
- State mutations must occur via explicit domain methods (e.g., `VerificarPassword`), never via public property setters.
- Domain entities must have zero external dependencies apart from core domain logic packages (e.g. `BCrypt.Net`).

## 2. Infrastructure & EF Core Performance
- Read-only queries in `BillingRadar.Infrastructure.Repositories` MUST explicitly append `.AsNoTracking()` to avoid unnecessary change-tracking overhead.
  - Baseline violation: `GetUserByEmailAsync` in `UserRepository.cs:L19` (`_context.Users.FirstOrDefaultAsync(...)` without `.AsNoTracking()`).
- Repositories must return nullable domain entities (`User?`) or collections, wrapping raw database interactions.

## 3. Application Layer & CQRS Conventions
- Use cases must reside under `BillingRadar.Application.Modules.{ModuleName}.[Command|Query]`.
- Requests must be C# `record` types implementing `IRequest<Result<TResponse>>`.
- Responses and DTOs MUST be C# `record` types (immutable records).
- Responses must return the custom `Result<T>` wrapper (`BillingRadar.Application.Shared.Result`).
- Handlers must implement `IRequestHandler<TRequest, Result<TResponse>>`.
- Domain entities must NEVER be exposed in Request, Response, or DTO signatures.

## 4. Technical Debt Tracking (Known Legacy Violations)
- **`LoginQueryResponse.cs`** (`BillingRadar.Application/Modules/Auth/Query/LoginQueryResponse.cs:L3`):
  - Current state: `public class LoginQueryResponse(string AccessToken, string RefreshToken);`
  - Required Refactoring (Future PR):
    1. Convert from `class` to `record`.
    2. Change primary constructor parameter names from PascalCase (`AccessToken`, `RefreshToken`) to standard C# camelCase (`accessToken`, `refreshToken`).

## 5. Dependency Injection Organization
- Application services and MediatR/FluentValidation handlers must be registered via `AddApplication()` in `BillingRadar.Application.DependencyInjection.DependencyInjection`.
- Infrastructure persistence, EF Core DbContext, and repositories must be registered via `AddInfrastructure()` in `BillingRadar.Infrastructure.DependencyInjection.DependencyInjection`.

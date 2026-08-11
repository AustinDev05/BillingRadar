---
name: ef-query-audit
description: Audits Entity Framework Core query performance, AsNoTracking usage, and potential N+1 or client-side evaluation issues in BillingRadar.Infrastructure. Triggers when asked to "revisar repositorio", "optimizar query", "auditar EF Core", or "antes de mergear cambios en Infrastructure".
---

# EF Core Query Audit Instructions

When auditing repositories in `BillingRadar.Infrastructure/Repositories/`, execute the following checks to identify performance anti-patterns:

## 1. Check Read-Only Queries for Missing `.AsNoTracking()`
Search for methods performing read operations (e.g. `Get*`, `Find*`, `List*`, `FirstOrDefaultAsync`, `ToListAsync`) and ensure they use `.AsNoTracking()`.

- **Known Baseline Finding**: `BillingRadar.Infrastructure/Repositories/UserRepository.cs:L19`:
  ```csharp
  // Non-compliant: Missing .AsNoTracking()
  return _context.Users.FirstOrDefaultAsync(u => u.Email == email);

  // Compliant:
  return _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
  ```

## 2. Check for Missing `.Include()` (N+1 Queries)
Ensure related entities needed by the caller are explicitly loaded with `.Include(...)` instead of triggering multiple database queries.

## 3. Check for Premature In-Memory Evaluation
Scan for `.ToList()`, `.ToArray()`, or `.AsEnumerable()` being invoked prior to `.Where(...)` or `.Select(...)` filters, which causes all rows to be loaded into application memory before filtering.

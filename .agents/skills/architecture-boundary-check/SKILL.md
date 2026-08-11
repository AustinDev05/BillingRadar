---
name: architecture-boundary-check
description: Audits project references and Clean Architecture layer boundaries in BillingRadar. Triggers when asked to "revisar arquitectura", "validar dependencias entre capas", "verificar limites", or "antes de hacer PR".
---

# Architecture Boundary Check Instructions

When executed, you MUST run the boundary verification script to validate that Clean Architecture principles are respected across all 4 projects:

## Execution Steps

1. Run the boundary check script from the terminal:
   ```powershell
   powershell -ExecutionPolicy Bypass -File .agents/skills/architecture-boundary-check/scripts/check-boundaries.ps1
   ```
2. Review the command output.
3. If any project reference violates Clean Architecture (e.g., Domain referencing Application/Infrastructure, or Application referencing Infrastructure/WebAPI), report the violation explicitly and abort the PR or build process until resolved.

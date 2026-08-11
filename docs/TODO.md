# BillingRadar Roadmap & Technical TODO

Este documento reúne la hoja de ruta técnica, hallazgos de auditoría y los aprendizajes/patrones extraídos de `agRepuestos` para continuar la evolución de la arquitectura en **BillingRadar**.

---

## 1. Arquitectura & CQRS (Application Layer)

### 1.1 Estándar de Mensajería y Respuestas
- [x] **Definición de Respuestas e Inmutabilidad**: Convertir respuestas y DTOs a `positional records` inmutables (ej. `CreateUserCommandResponse`, `UserQueryResponse`).
- [x] **Contratos CQRS**: Asegurar que todas las peticiones implementen `ICommand<T>` o `IQuery<T>` definidas en [`ICQRS.cs`](file:///C:/Users/Austin/Proyectos/BillingRadar/BillingRadar.Application/Shared/ICQRS.cs), evitando la referencia directa a `IRequest<Result<T>>`.
- [x] **Defensa contra Nulos**: Implementar validaciones defensivas (`Result.Failure`) en Handlers de consulta cuando las búsquedas en el repositorio devuelvan `null`.

### 1.2 Pipeline de Validación Automática
- [ ] **MediatR ValidationBehavior**: Inspirado en `agRepuestos`, crear un `ValidationBehavior<TRequest, TResponse>` en `BillingRadar.Application/Shared/Behaviors` que ejecute automáticamente los validadores de `FluentValidation` antes de llamar a los Handlers.
- [ ] **Registro en DI**: Registrar el comportamiento en `AddApplication()` de `BillingRadar.Application.DependencyInjection`.

---

## 2. Autenticación & Refresh Tokens (Modulo Auth)

### 2.1 Dominio y Persistencia (PostgreSQL)
- [ ] **Entidad `RefreshToken`**:
  - Definir la entidad o tipo complejo en `BillingRadar.Domain.Entities.RefreshToken` con encapsulamiento estricto (`private set`).
  - Incluir propiedades: `Token`, `UserId`, `ExpiresAt`, `RevokedAt`, `IsExpired`, `IsActive`.
- [ ] **Configuración EF Core**:
  - Mapear la relación en `BillingRadar.Infrastructure` entre `User` y `RefreshToken`.
  - Crear y ejecutar migración de EF Core: `dotnet ef migrations add AddRefreshToken`.

### 2.2 Aplicación (Casos de Uso)
- [ ] **Extensión de `LoginQuery` / `LoginCommandHandler`**:
  - Generar tanto `AccessToken` como `RefreshToken` al iniciar sesión.
  - Persistir el `RefreshToken` en PostgreSQL.
- [ ] **Comando `RefreshTokenCommand`**:
  - Crear `RefreshTokenCommand(string refreshToken) : ICommand<LoginQueryResponse>`.
  - Implementar `RefreshTokenHandler`:
    - Validar existencia y estado activo del token.
    - Implementar **RefreshToken Rotation (RTR)**: Revocar/eliminar el token anterior al emitir el nuevo par.

---

## 3. Lecciones Aprendidas y Reutilización desde `agRepuestos`

1. **Diferencia de Persistencia**:
   - `agRepuestos` almacena los Refresh Tokens en Redis mediante `IDistributedCache`.
   - **BillingRadar** utilizará la base de datos PostgreSQL principal para evitar dependencias de infraestructura adicionales.
2. **Validación Automática en MediatR**:
   - Copiar la estrategia de interceptación con `ValidationBehavior` para evitar escribir código repetitivo de validación dentro de los Handlers.
3. **Respeto de Limites de Capa**:
   - El Dominio no debe conocer de `Result<T>` ni depender de paquetes externos fuera de utilidades esenciales de dominio.

---

## 4. Deuda Técnica y Optimización
- [ ] Refactorizar la advertencia de nulabilidad `CS8618` en DTOs restantes.
- [ ] Revisar consultas de lectura en repositorios para asegurar la inclusión explícita de `.AsNoTracking()` conforme a las reglas del espacio de trabajo.

# Convenciones de Arquitectura CQRS en BillingRadar

Este documento define la estructura, reglas y el flujo secuencial para implementar casos de uso utilizando el patrón CQRS con MediatR.

---

## 1. Reglas de Inmutabilidad y Tipos

* **Requests (Commands / Queries)**: Deben definirse obligatoriamente como tipos `record` en C#. Esto garantiza la inmutabilidad de la petición desde la capa de entrada (API).
* **Responses / DTOs**: Deben definirse como tipos `record` con propiedades de solo lectura utilizando constructores primarios (PascalCase en propiedades públicas externas, o camelCase en constructores primarios).
* **Entidades de Dominio**: Nunca deben ser expuestas en firmas de Requests, Responses o DTOs.

---

## 2. Abstracciones de Interfaces

Las abstracciones principales están ubicadas en [ICQRS.cs](file:///C:/Users/Austin/Proyectos/BillingRadar/BillingRadar.Application/Shared/ICQRS.cs).

| Acción | Tipo de Request | Interfaz de Request | Interfaz de Handler (MediatR) |
| :--- | :--- | :--- | :--- |
| **Comando sin retorno** | Record | `ICommand` | `IRequestHandler<TCommand, Result>` |
| **Comando con retorno** | Record | `ICommand<TResponse>` | `IRequestHandler<TCommand, Result<TResponse>>` |
| **Consulta (Query)** | Record | `IQuery<TResponse>` | `IRequestHandler<TQuery, Result<TResponse>>` |

> [!WARNING]
> No existen interfaces personalizadas como `ICommandHandler` o `IQueryHandler` en el proyecto. Utiliza siempre `IRequestHandler` de MediatR para los manejadores.

---

## 3. Flujo de Creación Paso a Paso (Ejemplo: Módulo User)

Para añadir un nuevo caso de uso, sigue este orden recomendado:

### Paso 1: Definir el DTO de Respuesta (Response)
Crea el record inmutable para la respuesta en la subcarpeta del comando/query correspondiente.
```csharp
namespace BillingRadar.Application.Modules.User.Command
{
    public record CreateUserCommandResponse(Guid Id, string Email);
}
```

### Paso 2: Definir el Request (Command o Query)
Implementa la interfaz adecuada (`ICommand<T>` o `IQuery<T>`).
```csharp
using BillingRadar.Application.Shared;

namespace BillingRadar.Application.Modules.User.Command
{
    public record CreateUserCommand(string Email, string Password) : ICommand<CreateUserCommandResponse>;
}
```

### Paso 3: Crear el Handler (Manejador)
Implementa `IRequestHandler<TRequest, Result<TResponse>>` resolviendo la lógica y retornando el wrapper `Result`.
```csharp
using MediatR;
using BillingRadar.Application.Shared;

namespace BillingRadar.Application.Modules.User.Command
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserCommandResponse>>
    {
        public async Task<Result<CreateUserCommandResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Lógica de negocio (ej. hash de password, validaciones, guardar en DB)
            return Result<CreateUserCommandResponse>.Success(new CreateUserCommandResponse(Guid.NewGuid(), request.Email));
        }
    }
}
```

---

## 4. Errores Comunes y Soluciones

### Error: `CS0246: El nombre del tipo o del espacio de nombres 'IQueryHandler' no se encontró`
* **Causa**: Intentar usar `IQueryHandler` o `ICommandHandler` pensando que están definidos en el proyecto.
* **Solución**: Reemplazar por `IRequestHandler<TRequest, Result<TResponse>>` de MediatR.

### Error: `CS0311: El tipo 'Command' no se puede usar como parámetro de tipo 'TRequest'`
* **Causa**: El tipo de retorno especificado en la firma de tu comando (ej. `IRequest<Result<Unit>>`) no coincide con el tipo de retorno especificado en el handler (ej. `IRequestHandler<Command, Result<Response>>`).
* **Solución**: Asegúrate de que tanto el comando/query como el handler usen exactamente el mismo tipo de `Result<T>`.

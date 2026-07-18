1. Inicialización del Backend (.NET 10 / Clean Architecture)
Para no crear los 4 proyectos manualmente uno a uno y enlazarlos, usaremos una serie de comandos encadenados. Ejecuta esto en tu terminal:

Bash
# 1. Crear la solución de .NET
dotnet new sln -n BillingRadar

# 2. Crear los 4 proyectos de Clean Architecture
dotnet new classlib -n BillingRadar.Domain -f net9.0
dotnet new classlib -n BillingRadar.Application -f net9.0
dotnet new classlib -n BillingRadar.Infrastructure -f net9.0
dotnet new webapi -n BillingRadar.WebAPI -f net9.0

# 3. Agregar los proyectos a la solución (.sln)
dotnet sln add BillingRadar.Domain/BillingRadar.Domain.csproj
dotnet sln add BillingRadar.Application/BillingRadar.Application.csproj
dotnet sln add BillingRadar.Infrastructure/BillingRadar.Infrastructure.csproj
dotnet sln add BillingRadar.WebAPI/BillingRadar.WebAPI.csproj

# 4. Establecer las referencias de dependencia entre capas
# Application depende solo de Domain
dotnet add BillingRadar.Application/BillingRadar.Application.csproj reference BillingRadar.Domain/BillingRadar.Domain.csproj

# Infrastructure depende de Application (y por transitividad de Domain)
dotnet add BillingRadar.Infrastructure/BillingRadar.Infrastructure.csproj reference BillingRadar.Application/BillingRadar.Application.csproj

# WebAPI (Presentación) depende de Infrastructure y Application
dotnet add BillingRadar.WebAPI/BillingRadar.WebAPI.csproj reference BillingRadar.Infrastructure/BillingRadar.Infrastructure.csproj
dotnet add BillingRadar.WebAPI/BillingRadar.WebAPI.csproj reference BillingRadar.Application/BillingRadar.Application.csproj
Paquetes NuGet Esenciales (Opcional pero recomendado de una vez)
Si quieres dejar instalados los paquetes clave que mencionamos en la hoja de ruta, ejecuta esto:

Bash
# MediatR para CQRS (Se instala en Application)
dotnet add BillingRadar.Application/BillingRadar.Application.csproj package MediatR

# Entity Framework Core y Postgres (Se instalan en Infrastructure)
dotnet add BillingRadar.Infrastructure/BillingRadar.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add BillingRadar.Infrastructure/BillingRadar.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL

# Paquete para JWT (Se instala en WebAPI o Infrastructure, según prefieras)
dotnet add BillingRadar.WebAPI/BillingRadar.WebAPI.csproj package Microsoft.AspNetCore.Authentication.JwtBearer
2. Inicialización del Frontend (Vue.js + Vite)
Para el frontend, nos paramos en la raíz de la carpeta BillingRadar (al lado de la carpeta de la solución de .NET) y ejecutamos el inicializador oficial de Vite:

Bash
npm create vue@latest billingradar-front
Al ejecutarlo, la CLI de Vue te hará una serie de preguntas. Te recomiendo responder así para dejar el ecosistema listo:

Project name: billingradar-front (Ya viene por defecto)

Add TypeScript? A tu elección (Si vienes de C#, TypeScript te hará sentir como en casa con el tipado fuerte).

Add JSX Support? No.

Add Vue Router for Single Page Application development? Sí (Lo necesitas para moverte entre el Login y el Dashboard).

Add Pinia for state management? Sí (Crucial para manejar el estado de las finanzas y balances).

Add Vitest for Unit Testing? No (Puedes agregarlo después si quieres).

Add an End-to-End Testing Solution? No.

Add ESLint for code quality? Sí.

Add Prettier for code formatting? Sí.

Una vez termine, ejecuta los comandos para instalar las dependencias levantar el entorno de desarrollo:
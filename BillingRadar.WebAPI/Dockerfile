# ==============================================================================
# BillingRadar Backend - .NET 10 Dockerfile
# ==============================================================================

# Stage 1: Build & Publish
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /src

# Copy the modern solution file and projects
COPY BillingRadar.slnx ./
COPY BillingRadar.Domain/BillingRadar.Domain.csproj BillingRadar.Domain/
COPY BillingRadar.Application/BillingRadar.Application.csproj BillingRadar.Application/
COPY BillingRadar.Infrastructure/BillingRadar.Infrastructure.csproj BillingRadar.Infrastructure/
COPY BillingRadar.WebAPI/BillingRadar.WebAPI.csproj BillingRadar.WebAPI/

# Restore dependencies
RUN dotnet restore

# Copy all source files and publish
COPY . .
RUN dotnet publish BillingRadar.WebAPI/BillingRadar.WebAPI.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/publish .

# Environment settings
ENV ASPNETCORE_URLS=http://+:5140
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5140

ENTRYPOINT ["dotnet", "BillingRadar.WebAPI.dll"]

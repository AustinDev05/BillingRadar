# PowerShell script to verify project references and architecture boundaries for BillingRadar

$ErrorActionPreference = "Stop"

# Navigate 5 levels up from script location to reach repository root
$targetPath = Join-Path $PSScriptRoot "..\..\..\..\.."
if (Test-Path (Join-Path $targetPath "BillingRadar.slnx")) {
    $rootPath = (Resolve-Path $targetPath).Path
} else {
    # Fallback to 4 levels if 5 levels steps past repo root
    $targetPath4 = Join-Path $PSScriptRoot "..\..\..\.."
    if (Test-Path (Join-Path $targetPath4 "BillingRadar.slnx")) {
        $rootPath = (Resolve-Path $targetPath4).Path
    } else {
        $rootPath = (Resolve-Path $targetPath).Path
    }
}
Set-Location $rootPath

Write-Host "=== Checking Architectural Dependencies in BillingRadar ===" -ForegroundColor Cyan
Write-Host "Working Directory: $rootPath" -ForegroundColor Gray

$hasErrors = $false

# 1. Domain must not reference any project
$domainRefs = dotnet list BillingRadar.Domain/BillingRadar.Domain.csproj reference
if ($domainRefs -match "BillingRadar\.") {
    Write-Host "[ERROR] BillingRadar.Domain must not reference any other internal project!" -ForegroundColor Red
    $hasErrors = $true
} else {
    Write-Host "[OK] BillingRadar.Domain has clean architecture boundaries." -ForegroundColor Green
}

# 2. Application must only reference Domain
$appRefs = dotnet list BillingRadar.Application/BillingRadar.Application.csproj reference
if ($appRefs -match "BillingRadar\.Infrastructure" -or $appRefs -match "BillingRadar\.WebAPI") {
    Write-Host "[ERROR] BillingRadar.Application cannot reference Infrastructure or WebAPI!" -ForegroundColor Red
    $hasErrors = $true
} else {
    Write-Host "[OK] BillingRadar.Application references are valid." -ForegroundColor Green
}

# 3. WebAPI must NOT reference Domain directly
$apiRefs = dotnet list BillingRadar.WebAPI/BillingRadar.WebAPI.csproj reference
if ($apiRefs -match "BillingRadar\.Domain") {
    Write-Host "[ERROR] BillingRadar.WebAPI must NOT reference BillingRadar.Domain directly! (Clean Architecture violation)" -ForegroundColor Red
    $hasErrors = $true
} else {
    Write-Host "[OK] BillingRadar.WebAPI does not reference Domain directly." -ForegroundColor Green
}

# 4. WebAPI must reference Application and Infrastructure
if ($apiRefs -notmatch "BillingRadar\.Application" -or $apiRefs -notmatch "BillingRadar\.Infrastructure") {
    Write-Host "[ERROR] BillingRadar.WebAPI must reference both Application and Infrastructure!" -ForegroundColor Red
    $hasErrors = $true
} else {
    Write-Host "[OK] BillingRadar.WebAPI references Application and Infrastructure correctly." -ForegroundColor Green
}

if ($hasErrors) {
    Write-Host "`nArchitecture boundary validation FAILED!" -ForegroundColor Red
    exit 1
} else {
    Write-Host "`nAll architectural boundary checks PASSED successfully." -ForegroundColor Green
    exit 0
}

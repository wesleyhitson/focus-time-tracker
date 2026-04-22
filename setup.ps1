# Focus Time Tracker - Setup Script
# This script prepares and validates the current .NET 9 Avalonia project.

Write-Host "=== Focus Time Tracker Setup ===" -ForegroundColor Cyan
Write-Host ""

$projectPath = "FocusTimeTracker"
$projectFile = Join-Path $projectPath "FocusTimeTracker.csproj"

if (-not (Test-Path $projectFile)) {
    Write-Host "ERROR: Could not find project file at $projectFile" -ForegroundColor Red
    Write-Host "Run this script from the repository root." -ForegroundColor Yellow
    exit 1
}

# Check if dotnet is installed
Write-Host "Checking .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: .NET SDK not found. Install .NET 9 SDK from https://dot.net" -ForegroundColor Red
    exit 1
}

Write-Host "OK: .NET SDK $dotnetVersion found" -ForegroundColor Green

$dotnetMajor = 0
if ([int]::TryParse(($dotnetVersion -split '\.')[0], [ref]$dotnetMajor) -and $dotnetMajor -lt 9) {
    Write-Host "WARNING: .NET 9 SDK is recommended for this project." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
Push-Location $projectPath
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Write-Host "ERROR: dotnet restore failed." -ForegroundColor Red
    exit 1
}
Write-Host "OK: Restore completed" -ForegroundColor Green

Write-Host ""
Write-Host "Building project..." -ForegroundColor Yellow
dotnet build
if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Write-Host "ERROR: dotnet build failed." -ForegroundColor Red
    exit 1
}
Write-Host "OK: Build completed" -ForegroundColor Green
Pop-Location

Write-Host ""
Write-Host "=== Setup Complete ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor White
Write-Host "1. Open FocusTimeTracker.sln in Visual Studio 2022 or newer" -ForegroundColor Gray
Write-Host "2. Set FocusTimeTracker as startup project" -ForegroundColor Gray
Write-Host "3. Press F5 to run" -ForegroundColor Gray
Write-Host ""
Write-Host "Command-line run:" -ForegroundColor White
Write-Host "  cd FocusTimeTracker" -ForegroundColor Gray
Write-Host "  dotnet run" -ForegroundColor Gray
Write-Host ""
Write-Host "Command-line publish (Windows x64):" -ForegroundColor White
Write-Host "  cd FocusTimeTracker" -ForegroundColor Gray
Write-Host "  dotnet publish -c Release -r win-x64" -ForegroundColor Gray
Write-Host ""
Write-Host "See QUICKSTART.md for full usage details." -ForegroundColor Cyan

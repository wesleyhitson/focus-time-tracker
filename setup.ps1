# Focus Time Tracker - Setup Script
# This script will help you set up the development environment

Write-Host "=== Focus Time Tracker Setup ===" -ForegroundColor Cyan
Write-Host ""

# Check if dotnet is installed
Write-Host "Checking .NET installation..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "? .NET SDK not found! Please install .NET 8.0 SDK from https://dot.net" -ForegroundColor Red
    exit 1
}
Write-Host "? .NET SDK $dotnetVersion found" -ForegroundColor Green
Write-Host ""

# Check MAUI workload
Write-Host "Checking MAUI workload..." -ForegroundColor Yellow
$workloads = dotnet workload list 2>&1 | Out-String
if ($workloads -match "maui") {
    Write-Host "? MAUI workload already installed" -ForegroundColor Green
} else {
    Write-Host "??  MAUI workload not found. Installing..." -ForegroundColor Yellow
    Write-Host "   This may take 5-10 minutes..." -ForegroundColor Gray
    
    $choice = Read-Host "Install MAUI workload? (Y/n)"
    if ($choice -ne "n" -and $choice -ne "N") {
        dotnet workload install maui
        if ($LASTEXITCODE -eq 0) {
            Write-Host "? MAUI workload installed successfully" -ForegroundColor Green
        } else {
            Write-Host "? Failed to install MAUI workload" -ForegroundColor Red
            Write-Host "   Try running as Administrator or install via Visual Studio" -ForegroundColor Gray
        }
    }
}
Write-Host ""

# Check for fonts
Write-Host "Checking fonts..." -ForegroundColor Yellow
$fontsPath = "FocusTimeTracker\Resources\Fonts"
$regularFont = Join-Path $fontsPath "OpenSans-Regular.ttf"
$semiboldFont = Join-Path $fontsPath "OpenSans-Semibold.ttf"

if ((Test-Path $regularFont) -and ((Get-Item $regularFont).Length -gt 1000)) {
    Write-Host "? Fonts already downloaded" -ForegroundColor Green
} else {
    Write-Host "??  Fonts not found. The app can run with system fonts," -ForegroundColor Yellow
    Write-Host "   but Open Sans fonts are recommended for best appearance." -ForegroundColor Gray
    Write-Host ""
    Write-Host "   Download fonts manually from: https://fonts.google.com/specimen/Open+Sans" -ForegroundColor Gray
    Write-Host "   Place OpenSans-Regular.ttf and OpenSans-Semibold.ttf in:" -ForegroundColor Gray
    Write-Host "   $fontsPath" -ForegroundColor Gray
}
Write-Host ""

# Restore packages
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
Push-Location "FocusTimeTracker"
dotnet restore 2>&1 | Out-Null
if ($LASTEXITCODE -eq 0) {
    Write-Host "? Packages restored successfully" -ForegroundColor Green
} else {
    Write-Host "??  Package restore had issues (may need MAUI workload)" -ForegroundColor Yellow
}
Pop-Location
Write-Host ""

# Summary
Write-Host "=== Setup Complete ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor White
Write-Host "1. Open FocusTimeTracker.sln in Visual Studio 2022" -ForegroundColor Gray
Write-Host "2. Select 'Windows Machine' as the debug target" -ForegroundColor Gray
Write-Host "3. Press F5 to run the app" -ForegroundColor Gray
Write-Host ""
Write-Host "Or run from command line:" -ForegroundColor White
Write-Host "  cd FocusTimeTracker" -ForegroundColor Gray
Write-Host "  dotnet run -f net8.0-windows10.0.19041.0" -ForegroundColor Gray
Write-Host ""
Write-Host "?? See QUICKSTART.md for detailed instructions" -ForegroundColor Cyan
Write-Host "?? Happy focusing!" -ForegroundColor Green

# Build and Publish Script for Focus Time Tracker
# Creates a self-contained single-file executable

Write-Host "Building Focus Time Tracker for Windows..." -ForegroundColor Cyan

# Clean previous builds
dotnet clean --configuration Release

# Publish as single-file executable
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true

Write-Host ""
Write-Host "Build Complete!" -ForegroundColor Green
Write-Host ""
Write-Host "Your executable is located at:" -ForegroundColor Yellow
Write-Host "FocusTimeTracker\bin\Release\net9.0\win-x64\publish\FocusTimeTracker.exe" -ForegroundColor White
Write-Host ""
Write-Host "File size: approximately 60-80 MB (includes .NET runtime)" -ForegroundColor Gray
Write-Host ""
Write-Host "You can now:" -ForegroundColor Cyan
Write-Host "  1. Copy the .exe to any Windows 10/11 computer" -ForegroundColor White
Write-Host "  2. Run it directly - no installation needed" -ForegroundColor White
Write-Host "  3. Distribute it however you like" -ForegroundColor White
Write-Host ""

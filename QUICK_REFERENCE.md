# ?? Quick Reference Card

## Essential Commands

### Setup (Run Once)
```powershell
# Install MAUI workload
dotnet workload install maui

# Or run automated setup
.\setup.ps1
```

### Build & Run
```powershell
# Open in Visual Studio
start FocusTimeTracker.sln

# Or command line
cd FocusTimeTracker
dotnet restore
dotnet run -f net8.0-windows10.0.19041.0
```

## Project Structure
```
FocusTimeTracker/
??? Models/              # Data classes
??? Services/            # Business logic
??? ViewModels/          # MVVM logic
??? Views/               # UI (XAML)
??? Platforms/           # Platform-specific code
??? Converters/          # Data binding helpers
??? Resources/           # Images, fonts, icons
```

## Key Files

| File | Purpose |
|------|---------|
| `MauiProgram.cs` | Dependency injection setup |
| `App.xaml` | App-wide styles and colors |
| `AppShell.xaml` | Navigation structure |
| `DatabaseService.cs` | SQLite operations |
| `TimerService.cs` | Focus timer logic |
| `TimerViewModel.cs` | Timer page logic |

## Database Location

```
Windows:   %LocalAppData%\FocusTimeTracker\focus_tracker.db
macOS:     ~/Library/Application Support/FocusTimeTracker/focus_tracker.db
Android:   /data/data/com.focustime.tracker/files/
```

## Customization Quick Tips

### Change Timer Default
`TimerViewModel.cs` line 18:
```csharp
private int _focusDuration = 25; // Change this number
```

### Change Primary Color
`App.xaml`:
```xml
<Color x:Key="Primary">#7C3AED</Color>
```

### Add New Statistic
1. Add method in `DatabaseService.cs`
2. Add property in `StatisticsViewModel.cs`
3. Add UI in `StatisticsPage.xaml`

## Platform-Specific Builds

```powershell
# Windows
dotnet build -f net8.0-windows10.0.19041.0

# Android
dotnet build -f net8.0-android

# iOS (requires Mac)
dotnet build -f net8.0-ios

# macOS
dotnet build -f net8.0-maccatalyst
```

## NuGet Packages Used

- `Microsoft.Maui.Controls` - UI framework
- `sqlite-net-pcl` - Database
- `SQLitePCLRaw.bundle_green` - SQLite native
- `CommunityToolkit.Mvvm` - MVVM helpers
- `CommunityToolkit.Maui` - Extra controls

## Common Issues

| Issue | Solution |
|-------|----------|
| Workload not found | `dotnet workload update` |
| Font errors | Download fonts or use system fonts |
| Android SDK missing | Install via VS Installer |
| Database not saving | Check AppData permissions |

## Documentation Files

- ?? **README.md** - Full documentation
- ?? **QUICKSTART.md** - Setup guide
- ??? **ARCHITECTURE.md** - Code structure
- ?? **PROJECT_SUMMARY.md** - What's included
- ?? **setup.ps1** - Automated setup

## Keyboard Shortcuts (Visual Studio)

- `F5` - Build and run
- `Shift+F5` - Stop debugging
- `Ctrl+Shift+B` - Build solution
- `Ctrl+R, Ctrl+R` - Rename symbol
- `F12` - Go to definition

## Data Model

### FocusSession Table
```sql
Id, StartTime, EndTime, DurationMinutes, 
ActualMinutes, Category, Completed, Notes
```

## MVVM Pattern
```
View (XAML) ? ViewModel ? Service ? Database
     ?           ?
     ?? Data Binding ??
```

## Quick Test Checklist

- [ ] App builds without errors
- [ ] Timer shows "25:00"
- [ ] Start button works
- [ ] Pause/resume works
- [ ] Stop saves to database
- [ ] History shows sessions
- [ ] Statistics display correctly
- [ ] Swipe to delete works
- [ ] Notification on completion

## Next Steps

1. Run `setup.ps1` to install dependencies
2. Open `FocusTimeTracker.sln` in Visual Studio
3. Press `F5` to run
4. Check `PROJECT_SUMMARY.md` for full details

## Support

- ?? Read the docs in the root folder
- ?? Common issues in `QUICKSTART.md`
- ??? Architecture details in `ARCHITECTURE.md`

---

**Ready to focus? Start the timer! ??**

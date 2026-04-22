# ?? Focus Time Tracker

A beautiful, cross-platform focus time tracking application built with .NET MAUI. Track your productivity, build focus habits, and visualize your progress - all while keeping your data 100% private and local.

![Focus Time Tracker](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![MAUI](https://img.shields.io/badge/MAUI-Cross--Platform-blue)
![SQLite](https://img.shields.io/badge/Storage-SQLite-003B57?logo=sqlite)

## ? Features

### ?? Core Functionality
- **Customizable Focus Timer**: Set focus sessions from 5 to 120 minutes
- **Smart Tracking**: Automatically saves completed and interrupted sessions
- **Category Labels**: Organize sessions by task or project
- **Pause & Resume**: Take breaks without losing your progress
- **Completion Notifications**: Get notified when your focus session ends

### ?? Progress Visualization
- **Daily Statistics**: See your focus time broken down by day
- **Time Period Views**: Analyze 7, 30, or 90-day trends
- **Completion Rate**: Track how often you complete full sessions
- **Total Metrics**: View lifetime focus time and session counts

### ?? Privacy First
- **100% Local Storage**: All data stored on your device using SQLite
- **No Account Required**: Zero sign-up, zero cloud sync
- **Offline First**: Works completely without internet
- **No Telemetry**: Your data stays yours

### ?? Cross-Platform
- ? Windows 10/11
- ? macOS (via Mac Catalyst)
- ? iOS 11+
- ? Android 5.0+ (API 21)
- ?? Linux (possible via Avalonia in future)

## ??? Screenshots

### Timer Page
Beautiful circular timer with customizable durations and category tracking.

### History Page
Swipe-to-delete session list with completion indicators.

### Statistics Page
Comprehensive statistics with daily breakdowns and visual progress bars.

## ?? Getting Started

### Prerequisites
- **Visual Studio 2022** (17.8 or later)
- **.NET 8.0 SDK** or later
- **Workloads**:
  - .NET Multi-platform App UI development
  - For iOS/Mac: Mac with Xcode
  - For Android: Android SDK

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/focus-time-tracker.git
   cd focus-time-tracker/FocusTimeTracker
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Run the application**

   **For Windows:**
   ```bash
   dotnet build -f net8.0-windows10.0.19041.0
   dotnet run -f net8.0-windows10.0.19041.0
   ```

   **For Android:**
   ```bash
   dotnet build -f net8.0-android
   dotnet run -f net8.0-android
   ```

   **For iOS/Mac:**
   ```bash
   dotnet build -f net8.0-maccatalyst
   dotnet run -f net8.0-maccatalyst
   ```

   Or simply **F5 in Visual Studio** and select your target platform.

## ??? Architecture

### Tech Stack
- **Framework**: .NET MAUI 8.0
- **Language**: C# 12
- **UI Pattern**: MVVM with CommunityToolkit.Mvvm
- **Database**: SQLite with sqlite-net-pcl
- **Design**: Fluent-inspired dark theme

### Project Structure
```
FocusTimeTracker/
??? Models/              # Data models (FocusSession, DailyStatistic)
??? Services/            # Business logic (Timer, Database, Notifications)
??? ViewModels/          # MVVM view models
??? Views/               # XAML pages (Timer, History, Statistics)
??? Converters/          # Value converters for data binding
??? Platforms/           # Platform-specific implementations
?   ??? Windows/         # Windows notifications
?   ??? Android/         # Android notifications
?   ??? iOS/             # iOS notifications
?   ??? MacCatalyst/     # macOS notifications
??? Resources/           # Images, fonts, icons
```

### Key Components

**Services:**
- `DatabaseService`: SQLite operations and data queries
- `TimerService`: Focus session timer with events
- `INotificationService`: Platform-specific notifications

**Models:**
- `FocusSession`: Individual focus session record
- `DailyStatistic`: Aggregated daily statistics

**ViewModels:**
- `TimerViewModel`: Timer page logic
- `HistoryViewModel`: Session history with delete
- `StatisticsViewModel`: Statistics and charts

## ?? Dependencies

```xml
<PackageReference Include="Microsoft.Maui.Controls" Version="8.0.90" />
<PackageReference Include="sqlite-net-pcl" Version="1.9.172" />
<PackageReference Include="SQLitePCLRaw.bundle_green" Version="2.1.8" />
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" />
<PackageReference Include="CommunityToolkit.Maui" Version="9.0.3" />
```

## ?? Data Storage

### Database Location
Data is stored in a SQLite database at:

- **Windows**: `%LocalAppData%\FocusTimeTracker\focus_tracker.db`
- **macOS**: `~/Library/Application Support/FocusTimeTracker/focus_tracker.db`
- **Linux**: `~/.local/share/FocusTimeTracker/focus_tracker.db`
- **iOS**: App sandbox container
- **Android**: `/data/data/com.focustime.tracker/files/`

### Schema
```sql
CREATE TABLE focus_sessions (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME,
    DurationMinutes INTEGER NOT NULL,
    ActualMinutes INTEGER,
    Category TEXT NOT NULL,
    Completed BOOLEAN NOT NULL,
    Notes TEXT
);
```

### Backup Your Data
Simply copy the `focus_tracker.db` file to back up all your sessions!

## ?? Customization

### Change Theme Colors
Edit `App.xaml` to customize the color scheme:

```xml
<Color x:Key="Primary">#7C3AED</Color>     <!-- Purple -->
<Color x:Key="Secondary">#3B82F6</Color>   <!-- Blue -->
<Color x:Key="Success">#4CAF50</Color>     <!-- Green -->
```

### Default Focus Duration
Modify `TimerViewModel.cs`:

```csharp
[ObservableProperty]
private int _focusDuration = 25; // Change this value
```

### Notification Messages
Customize in `TimerViewModel.OnTimerCompleted()`:

```csharp
await _notificationService.ShowNotificationAsync(
    "Your Custom Title",
    "Your custom message here");
```

## ?? Development

### Adding New Features

1. **New Statistics**: Add methods to `DatabaseService.cs`
2. **New UI Pages**: Create XAML in `Views/` with ViewModel
3. **Platform Features**: Implement in `Platforms/{Platform}/`

### Running Tests
```bash
dotnet test
```

### Building for Release

**Windows:**
```bash
dotnet publish -f net8.0-windows10.0.19041.0 -c Release
```

**Android APK:**
```bash
dotnet publish -f net8.0-android -c Release
```

**iOS (requires Mac):**
```bash
dotnet publish -f net8.0-ios -c Release
```

## ?? Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## ?? License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## ?? Acknowledgments

- Inspired by the **Forest app** and **Pomodoro Technique**
- Built with [.NET MAUI](https://dotnet.microsoft.com/apps/maui)
- Icons and design inspired by **Fluent Design System**
- Community toolkit by [.NET Foundation](https://dotnetfoundation.org/)

## ?? Contact

Have questions or suggestions? Open an issue on GitHub!

---

**Made with ?? and ? by developers who value focus and privacy**

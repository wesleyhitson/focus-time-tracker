# ⚡ Quick Reference

## 🚀 Run Commands

```powershell
cd FocusTimeTracker
dotnet restore
dotnet build
dotnet run
```

## 🧰 Useful Commands

```powershell
# Build Release
dotnet build -c Release

# Publish Windows x64 single-file build
dotnet publish -c Release -r win-x64
```

## 📁 Core Folders

- `Models/`: Data entities
- `Services/`: Timer, database, and notification logic
- `ViewModels/`: MVVM state and command handling
- `Views/`: Avalonia UI views
- `Converters/`: UI value converters
- `Assets/`: App assets (icons/images)

## 🔑 Key Files

| File | Purpose |
|------|---------|
| `Program.cs` | Avalonia app bootstrap |
| `App.axaml.cs` | Service registration and app startup |
| `Views/MainWindow.axaml` | Main tabbed layout |
| `Services/TimerService.cs` | Timer behavior |
| `Services/DatabaseService.cs` | SQLite storage and queries |
| `ViewModels/TimerViewModel.cs` | Timer page state/commands |

## 📊 Session Data Fields

- `StartTime`
- `EndTime`
- `DurationMinutes`
- `ActualMinutes`
- `Category`
- `Completed`
- `Notes`

## 🧩 Feature Map

- ⏱️ Timer: Start/Pause/Resume/Stop
- 📚 History: Load and delete sessions
- 📈 Statistics: Totals + completion rate + daily trend
- 💾 Storage: Local SQLite
- 📤📥 CSV: Export and import

## 🛠️ Troubleshooting

- ❗ Build fails after package changes:
  - Run `dotnet restore`
- ❗ App does not start from root folder:
  - Run commands from `FocusTimeTracker/`
- ❗ No data available in stats/history:
  - Complete or stop at least one timer session first

## 📄 Related Docs

- `README.md`: High-level feature overview
- `QUICKSTART.md`: Step-by-step setup
- `ARCHITECTURE.md`: Design and code structure
- `PROJECT_SUMMARY.md`: Current implementation snapshot

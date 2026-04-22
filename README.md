# ⏱️ Focus Time Tracker

A cross-platform desktop focus timer built with .NET 9 and Avalonia UI.

Track focused work sessions, review history, analyze productivity stats, and export/import data via CSV.

## 🚀 Quick Start

From the repository root:

```powershell
cd FocusTimeTracker
dotnet restore
dotnet build
dotnet run
```

Publish a Windows x64 build:

```powershell
cd FocusTimeTracker
dotnet publish -c Release -r win-x64
```

## ✨ Features

- ▶️ **Start / Pause / Resume / Stop timer**: Run sessions with full control over progress.
- 🎯 **Preset durations**: Quickly choose common durations like 5, 25, or 60 minutes.
- ➕➖ **Minute-by-minute adjustment**: Fine-tune session length in 1-minute increments.
- 🏷️ **Session categories**: Label sessions (for example, General, Study, or Work).
- 📚 **Session history**: View saved sessions and remove individual entries.
- 📊 **Statistics dashboard**: See total focus time, session counts, completion rate, and daily stats.
- 📤 **CSV export**: Export all sessions to your Documents folder.
- 📥 **CSV import**: Import from the most recent FocusTimeTracker export CSV in Documents.
- 💾 **Local-first storage**: Save everything in a local SQLite database on your device.

## 🧱 Tech Stack

- .NET 9
- Avalonia UI 11
- CommunityToolkit.Mvvm
- SQLite (`sqlite-net-pcl`)
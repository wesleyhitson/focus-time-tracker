# ✅ Focus Time Tracker - Project Summary

## 📌 Overview

Focus Time Tracker is a desktop focus timer built with .NET 9 and Avalonia UI.

It helps you:
- ⏱️ Run timed focus sessions
- 🗂️ Track session history
- 📊 Review productivity statistics
- 💾 Keep data local in SQLite
- 📤📥 Export and import session data using CSV

## 🧱 Current Architecture

- 🖥️ UI Framework: Avalonia UI 11
- 🧠 Pattern: MVVM
- 💉 DI: Microsoft.Extensions.DependencyInjection
- 🗄️ Storage: SQLite via sqlite-net-pcl
- ⚙️ Runtime: .NET 9

## 📂 Main Project Structure

### Models
- `FocusSession.cs`: Individual session data (duration, category, completion, notes)
- `DailyStatistic.cs`: Aggregated stats used by charts/cards

### Services
- `TimerService.cs`: Timer lifecycle (start, pause, resume, stop, complete)
- `DatabaseService.cs`: SQLite initialization and CRUD/query operations
- `NotificationService.cs`: Notification abstraction implementation
- `INotificationService.cs`: Notification contract

### ViewModels
- `TimerViewModel.cs`: Timer controls, state, and progress updates
- `HistoryViewModel.cs`: Session history loading and delete operations
- `StatisticsViewModel.cs`: Totals, completion rate, and period-based daily stats
- `SettingsViewModel.cs`: CSV export/import behavior and status messages

### Views
- `MainWindow.axaml`: Tabbed shell (Timer, History, Statistics, Settings)
- `TimerView.axaml`: Focus timer UI
- `HistoryView.axaml`: Session list UI
- `StatisticsView.axaml`: Stats overview UI
- `SettingsView.axaml`: Data import/export UI

## ✨ Implemented Features

1. ▶️ Timer controls: start, pause, resume, and stop
2. 🎯 Preset durations and manual minute adjustments
3. 🏷️ Category-based session labeling
4. 📚 History with per-session deletion
5. 📊 Statistics for totals, completion rate, and recent daily activity
6. 📤 CSV export to Documents folder
7. 📥 CSV import from latest matching export in Documents
8. 🔒 Local-first data persistence in SQLite

## 🧪 Build Status Expectations

You should be able to:
- ✅ Restore NuGet packages
- ✅ Build the solution
- ✅ Run the desktop app from the `FocusTimeTracker` project

## 📝 Notes

- The codebase is now Avalonia desktop-first and no longer aligned with earlier MAUI-oriented docs.
- Setup and run guidance should use .NET 9 + Avalonia commands.

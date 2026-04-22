# 🚀 Quick Start Guide

## ✅ Prerequisites

Install:
- .NET 9 SDK
- Git (optional, if cloning)

Check your SDK:

```powershell
dotnet --version
```

## 1. 📦 Restore Dependencies

From the repository root:

```powershell
cd FocusTimeTracker
dotnet restore
```

## 2. 🔨 Build

```powershell
dotnet build
```

## 3. ▶️ Run

```powershell
dotnet run
```

The app launches as a desktop window with tabs for Timer, History, Statistics, and Settings.

## 4. 🧪 Quick Functional Check

1. Start a short timer
2. Pause and resume once
3. Stop or complete session
4. Open History tab and verify entry exists
5. Open Statistics tab and verify totals update

## 5. 📤 CSV Export / 📥 Import

In the Settings tab:
- Use Export to create a CSV in your Documents folder
- Use Import to load from the latest matching export file in Documents

## 6. 🏗️ Release Build

From the repository root:

```powershell
cd FocusTimeTracker
dotnet publish -c Release -r win-x64
```

## 🛠️ Common Fixes

### Build error after SDK update

```powershell
dotnet clean
dotnet restore
dotnet build
```

### Command works in one folder but not another

Run commands inside the `FocusTimeTracker` project directory.

### No history/statistics visible

A session must be saved first (by stopping or completing a timer).

## 📚 Next Docs

- `README.md` for features
- `ARCHITECTURE.md` for deeper implementation details
- `PROJECT_SUMMARY.md` for a concise architecture snapshot

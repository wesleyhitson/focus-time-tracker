# ?? Quick Start Guide

## Prerequisites

Before running the app, you need to install the .NET MAUI workloads.

### Step 1: Install MAUI Workloads

Open PowerShell or Command Prompt as Administrator and run:

```powershell
dotnet workload install maui
```

This will install all the necessary components for:
- Windows
- Android
- iOS
- macOS

**Note**: This may take 5-10 minutes depending on your internet connection.

### Step 2: Download Fonts

The app needs Open Sans fonts. Run these commands from the project root:

```powershell
cd FocusTimeTracker\Resources\Fonts
Invoke-WebRequest -Uri "https://github.com/google/fonts/raw/main/apache/opensans/OpenSans%5Bwdth%2Cwght%5D.ttf" -OutFile "OpenSans-Regular.ttf"
Copy-Item "OpenSans-Regular.ttf" "OpenSans-Semibold.ttf"
```

Or download manually from [Google Fonts](https://fonts.google.com/specimen/Open+Sans).

### Step 3: Restore and Build

```powershell
cd C:\Users\wesley.hitson\code\oneoffs\pomodoro-app\FocusTimeTracker
dotnet restore
dotnet build
```

### Step 4: Run the App

#### Option A: Visual Studio (Recommended)
1. Open `FocusTimeTracker.sln` in Visual Studio 2022
2. Select **Windows Machine** as the deployment target
3. Press **F5** or click **Debug > Start Debugging**

#### Option B: Command Line
```powershell
# For Windows
dotnet run -f net8.0-windows10.0.19041.0

# For Android (requires emulator or device)
dotnet run -f net8.0-android

# For Mac (requires Mac)
dotnet run -f net8.0-maccatalyst
```

## ?? Platform-Specific Setup

### Windows
? No additional setup required!

### Android
1. Install **Android SDK** via Visual Studio Installer
2. Create an emulator in **Android Device Manager**
3. Start the emulator before running

### iOS/macOS
1. Requires a **Mac** with Xcode installed
2. Pair with Mac in Visual Studio (Windows)
3. Or develop directly on Mac

## ?? Simplified Windows-Only Version

If you just want to run on Windows without installing all platforms, use the simplified project file:

Replace the `<TargetFrameworks>` line in `FocusTimeTracker.csproj` with:

```xml
<TargetFrameworks>net8.0-windows10.0.19041.0</TargetFrameworks>
```

Then you only need:
```powershell
dotnet workload install maui-windows
```

## ?? Troubleshooting

### "Workload not found"
Run: `dotnet workload update`

### "Android SDK not found"
Install via Visual Studio Installer > Modify > Mobile development with .NET

### "Mac connection failed"
Ensure Mac and Windows are on same network and Mac has Xcode + remote login enabled

### Font errors
The app will run with system fonts if custom fonts are missing, but download recommended

## ?? Platform Support Status

| Platform | Status | Requirements |
|----------|--------|--------------|
| Windows 10/11 | ? Ready | MAUI Windows workload |
| Android 5.0+ | ? Ready | Android SDK, Emulator |
| macOS | ? Ready | Mac with Xcode |
| iOS | ? Ready | Mac with Xcode + Apple Developer |

## ?? First Run

When you first run the app:
1. The timer page will show with default 25 minutes
2. Click **Start** to begin your first focus session
3. Sessions are automatically saved to SQLite database
4. View history and statistics in the other tabs

## ?? Data Location

Your focus session data is stored at:
- **Windows**: `%LocalAppData%\FocusTimeTracker\focus_tracker.db`
- **macOS**: `~/Library/Application Support/FocusTimeTracker/focus_tracker.db`
- **Android**: `/data/data/com.focustime.tracker/files/`

## ?? Need Help?

- Check [README.md](../README.md) for detailed architecture
- Review [FONTS_README.md](FocusTimeTracker/Resources/Fonts/FONTS_README.md) for font setup
- Open an issue on GitHub if you encounter problems

---

**Ready to start tracking your focus time? Let's build! ??**

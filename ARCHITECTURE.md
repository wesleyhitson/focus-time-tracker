# ??? Architecture Guide

## Overview

Focus Time Tracker is built using the **Model-View-ViewModel (MVVM)** pattern with .NET MAUI, ensuring clean separation of concerns and maximum code reusability across platforms.

## Technology Stack

```
???????????????????????????????????????
?         .NET MAUI 8.0               ?
?  (Cross-Platform UI Framework)      ?
???????????????????????????????????????
              ?
???????????????????????????????????????
?         XAML UI Layer               ?
?  (Views: Timer, History, Stats)     ?
???????????????????????????????????????
              ?
???????????????????????????????????????
?    ViewModels (MVVM Pattern)        ?
?  CommunityToolkit.Mvvm              ?
???????????????????????????????????????
              ?
???????????????????????????????????????
?      Business Logic Services        ?
?  Timer | Database | Notifications   ?
???????????????????????????????????????
              ?
???????????????????????????????????????
?      Data Layer (SQLite)            ?
?  sqlite-net-pcl (Local Storage)     ?
???????????????????????????????????????
```

## Project Structure

### ?? Models/
**Purpose**: Data models representing domain entities

- **`FocusSession.cs`**
  - Represents a single focus session
  - Properties: StartTime, EndTime, Duration, Category, Completed, Notes
  - Includes display helpers (DisplayDate, DisplayTime, StatusIcon)
  - SQLite table mapping via attributes

- **`DailyStatistic.cs`**
  - Aggregated statistics for a single day
  - Calculated from FocusSession data
  - Used for charts and progress visualization

### ?? Services/
**Purpose**: Business logic and cross-cutting concerns

#### `DatabaseService.cs`
- **Responsibilities**:
  - SQLite database initialization
  - CRUD operations for FocusSession
  - Aggregate queries (statistics, totals)
  - Data persistence
  
- **Key Methods**:
  ```csharp
  GetSessionsAsync()                    // Get all sessions
  GetSessionsByDateRangeAsync()         // Filtered by date
  SaveSessionAsync(session)             // Insert or update
  DeleteSessionAsync(session)           // Remove session
  GetDailyStatisticsAsync(days)         // Aggregated stats
  ```

- **Database Location**:
  - Windows: `%LocalAppData%/FocusTimeTracker/focus_tracker.db`
  - Uses `FileSystem.AppDataDirectory` for cross-platform paths

#### `TimerService.cs`
- **Responsibilities**:
  - Focus session countdown timer
  - State management (Running, Paused)
  - Event broadcasting
  
- **Events**:
  ```csharp
  Tick         // Fires every second with remaining time
  Completed    // Fires when session completes
  Started      // Fires when timer starts
  Paused       // Fires when timer pauses
  Resumed      // Fires when timer resumes
  ```

- **Implementation**:
  - Uses `System.Timers.Timer` for cross-platform timing
  - Tracks elapsed time vs target duration
  - Returns FocusSession on completion/stop

#### `INotificationService.cs` (Interface)
- **Responsibilities**:
  - Abstract notification API
  - Platform-specific implementations

- **Platform Implementations**:
  - **Windows**: `Microsoft.Windows.AppNotifications`
  - **macOS/iOS**: `UserNotifications` framework
  - **Android**: `NotificationCompat` and channels

### ?? ViewModels/
**Purpose**: Presentation logic and state management

#### `TimerViewModel.cs`
- **Features**:
  - Observable properties for UI binding
  - Timer controls (Start, Pause, Stop)
  - Duration adjustment commands
  - Progress calculation
  - Session auto-save on completion

- **Key Properties**:
  ```csharp
  DisplayTime      // "25:00" formatted string
  FocusDuration    // Minutes (5-120)
  IsRunning        // Timer state
  IsPaused         // Pause state
  ProgressValue    // 0.0 to 1.0 for circular progress
  ```

- **Commands**:
  - `StartTimerCommand` - Start/Pause/Resume
  - `StopTimerCommand` - Stop and save session
  - `IncreaseDurationCommand` - Add 5 minutes
  - `DecreaseDurationCommand` - Subtract 5 minutes

#### `HistoryViewModel.cs`
- **Features**:
  - Session list with observable collection
  - Delete functionality
  - Load on navigation
  - Empty state handling

- **Key Properties**:
  ```csharp
  Sessions         // ObservableCollection<FocusSession>
  IsLoading        // Loading indicator
  HasNoSessions    // Empty state visibility
  ```

#### `StatisticsViewModel.cs`
- **Features**:
  - Daily statistics with date range selection
  - Total metrics calculation
  - Completion rate tracking
  - Visual data for charts

- **Key Properties**:
  ```csharp
  DailyStats           // Daily breakdown
  TotalFocusMinutes    // Lifetime total
  TotalSessions        // Count of all sessions
  CompletedSessions    // Count of completed
  CompletionRate       // Percentage formatted
  SelectedDays         // 7, 30, or 90 day view
  ```

### ?? Views/
**Purpose**: XAML UI definitions

#### `TimerPage.xaml`
- Circular timer display with progress indicator
- Duration controls (+/- buttons)
- Category input field
- Start/Stop buttons with dynamic labels
- Focus tips section

#### `HistoryPage.xaml`
- `CollectionView` with session list
- Swipe-to-delete gesture
- Status indicators (? for complete, ? for incomplete)
- Empty state with helpful message

#### `StatisticsPage.xaml`
- Summary cards (4 metrics)
- Time period selector (7/30/90 days)
- Daily breakdown with progress bars
- Visual color coding

### ?? Converters/
**Purpose**: Value conversion for data binding

- **`StringNotNullOrEmptyConverter`**: Visibility based on string content
- **`SelectedButtonConverter`**: Highlight selected time period button
- **`MinutesToProgressConverter`**: Convert minutes to 0-1 progress value

### ?? Platforms/
**Purpose**: Platform-specific implementations

Each platform folder contains a `NotificationService.cs` implementing `INotificationService`:

```
Platforms/
??? Windows/        # AppNotifications API
??? Android/        # NotificationCompat + Channels
??? iOS/            # UNUserNotificationCenter
??? MacCatalyst/    # UNUserNotificationCenter
```

## Data Flow

### Starting a Focus Session
```
User taps "Start"
    ?
TimerViewModel.StartTimerCommand
    ?
TimerService.Start(duration, category)
    ?
Creates FocusSession in memory
    ?
System.Timers.Timer starts
    ?
Every second: Tick event ? Update UI
    ?
On completion: Completed event
    ?
DatabaseService.SaveSessionAsync()
    ?
SQLite Insert
    ?
NotificationService shows completion
```

### Viewing Statistics
```
User navigates to Statistics tab
    ?
StatisticsPage.OnAppearing()
    ?
StatisticsViewModel.LoadStatisticsAsync()
    ?
DatabaseService.GetDailyStatisticsAsync()
    ?
Query SQLite, group by date
    ?
Fill missing days with zero data
    ?
Update ObservableCollection
    ?
UI updates via data binding
```

## Dependency Injection

Configured in `MauiProgram.cs`:

```csharp
builder.Services.AddSingleton<DatabaseService>();    // Single instance
builder.Services.AddSingleton<TimerService>();       // Single instance
builder.Services.AddSingleton<INotificationService>(); // Platform-specific

builder.Services.AddTransient<TimerViewModel>();     // New per request
builder.Services.AddTransient<HistoryViewModel>();
builder.Services.AddTransient<StatisticsViewModel>();

builder.Services.AddTransient<TimerPage>();
builder.Services.AddTransient<HistoryPage>();
builder.Services.AddTransient<StatisticsPage>();
```

## MVVM Pattern Benefits

1. **Separation of Concerns**: UI (View) separate from logic (ViewModel)
2. **Testability**: ViewModels can be unit tested without UI
3. **Reusability**: ViewModels work across all platforms
4. **Maintainability**: Changes to UI don't affect business logic
5. **Data Binding**: Automatic UI updates via `INotifyPropertyChanged`

## CommunityToolkit.Mvvm Features Used

### Source Generators
```csharp
[ObservableProperty]  // Generates INotifyPropertyChanged boilerplate
private string _displayTime;

[RelayCommand]  // Generates ICommand implementation
private void StartTimer() { }
```

### Generated Code Example
```csharp
// You write:
[ObservableProperty]
private string _displayTime;

// Generator creates:
public string DisplayTime
{
    get => _displayTime;
    set
    {
        if (_displayTime != value)
        {
            _displayTime = value;
            OnPropertyChanged(nameof(DisplayTime));
        }
    }
}
```

## Database Schema

```sql
CREATE TABLE focus_sessions (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME,
    DurationMinutes INTEGER NOT NULL,      -- Target duration
    ActualMinutes INTEGER,                 -- Actual time spent
    Category TEXT NOT NULL DEFAULT 'General',
    Completed BOOLEAN NOT NULL DEFAULT 0,  -- 1 if full session
    Notes TEXT
);

-- Indexes for performance
CREATE INDEX idx_start_time ON focus_sessions(StartTime);
CREATE INDEX idx_completed ON focus_sessions(Completed);
```

## Performance Considerations

### Database Queries
- Indexes on `StartTime` and `Completed` columns
- Async operations prevent UI blocking
- Batched reads for statistics

### UI Updates
- `MainThread.BeginInvokeOnMainThread()` for timer updates
- ObservableCollection for efficient list updates
- Lazy loading of history/statistics on tab navigation

### Memory Management
- Singleton services for shared state
- Transient ViewModels to free memory when not in use
- Proper disposal of Timer instances

## Security & Privacy

1. **Local Storage Only**: No network requests, no cloud sync
2. **No Telemetry**: Zero analytics or tracking
3. **No Authentication**: No user accounts or login
4. **File Permissions**: Database only accessible to app
5. **Backup-Friendly**: Single `.db` file for easy backup

## Extension Points

Want to add features? Here's where to start:

### Add New Statistic
1. Add property to `StatisticsViewModel`
2. Add query method to `DatabaseService`
3. Add UI card to `StatisticsPage.xaml`

### Add Session Tags
1. Add `Tags` property to `FocusSession`
2. Add tag selector to `TimerPage.xaml`
3. Update database schema
4. Filter by tags in `HistoryViewModel`

### Add Break Timer
1. Create `BreakTimerService` (similar to `TimerService`)
2. Create `BreakTimerViewModel`
3. Add `BreakTimerPage` with short timer (5-10 min)
4. Auto-start after focus session completion

### Export Data
1. Add export method to `DatabaseService`:
   ```csharp
   public async Task<string> ExportToCsv()
   ```
2. Add button to Statistics page
3. Use `FileSaver` from CommunityToolkit.Maui

## Testing Strategy

### Unit Tests
- Test ViewModels with mocked services
- Test DatabaseService with in-memory SQLite
- Test TimerService events and state transitions

### UI Tests
- Use Appium for cross-platform UI tests
- Test navigation flow
- Test data binding

### Platform Tests
- Test notifications on each platform
- Test database file creation
- Test app lifecycle events

## Build Configuration

### Debug Build
- Full logging enabled
- Database in AppData (preserves data between runs)
- Hot reload enabled

### Release Build
- Optimizations enabled
- Logging reduced
- AOT compilation for performance
- Trimmed dependencies to reduce size

## Further Reading

- [.NET MAUI Documentation](https://learn.microsoft.com/dotnet/maui/)
- [MVVM Toolkit Documentation](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/)
- [SQLite-net Documentation](https://github.com/praeclarum/sqlite-net)
- [XAML Documentation](https://learn.microsoft.com/dotnet/maui/xaml/)

---

**Questions? Check the README.md or open an issue on GitHub!**

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FocusTimeTracker.Models;
using FocusTimeTracker.Services;
using Avalonia.Threading;

namespace FocusTimeTracker.ViewModels;

public partial class TimerViewModel : ObservableObject
{
    private readonly TimerService _timerService;
    private readonly DatabaseService _databaseService;
    private readonly INotificationService _notificationService;

    [ObservableProperty]
    private string _displayTime = "25:00";

    [ObservableProperty]
    private int _focusDuration = 25;

    [ObservableProperty]
    private string _category = "General";

    [ObservableProperty]
    private bool _isRunning = false;

    [ObservableProperty]
    private bool _isPaused = false;

    [ObservableProperty]
    private string _startButtonText = "Start";

    [ObservableProperty]
    private bool _canModifyDuration = true;

    [ObservableProperty]
    private double _progressValue = 0;

    public TimerViewModel(
        TimerService timerService, 
        DatabaseService databaseService,
        INotificationService notificationService)
    {
        _timerService = timerService;
        _databaseService = databaseService;
        _notificationService = notificationService;

        _timerService.Tick += OnTimerTick;
        _timerService.Completed += OnTimerCompleted;
        _timerService.Started += OnTimerStarted;
        _timerService.Paused += OnTimerPaused;
        _timerService.Resumed += OnTimerResumed;
    }

    [RelayCommand]
    private void StartTimer()
    {
        if (!IsRunning)
        {
            // Update display to show full duration before starting
            DisplayTime = $"{FocusDuration:D2}:00";
            _timerService.Start(FocusDuration, Category);
        }
        else if (IsPaused)
        {
            _timerService.Resume();
        }
        else
        {
            _timerService.Pause();
        }
    }

    [RelayCommand]
    private async Task StopTimer()
    {
        if (!IsRunning)
            return;

        var session = _timerService.Stop();
        await _databaseService.SaveSessionAsync(session);

        ResetTimer();
    }

    [RelayCommand]
    private void IncreaseDuration()
    {
        if (CanModifyDuration && FocusDuration < 120)
        {
            FocusDuration += 1;
            UpdateDisplayTime();
        }
    }

    [RelayCommand]
    private void DecreaseDuration()
    {
        if (CanModifyDuration && FocusDuration > 1)
        {
            FocusDuration -= 1;
            UpdateDisplayTime();
        }
    }

    [RelayCommand]
    private void SetPresetDuration(object parameter)
    {
        if (CanModifyDuration && parameter is string minutes && int.TryParse(minutes, out int duration))
        {
            FocusDuration = duration;
            UpdateDisplayTime();
        }
    }

    private void OnTimerTick(object? sender, TimeSpan remaining)
    {
        Dispatcher.UIThread.Post(() =>
        {
            DisplayTime = $"{(int)remaining.TotalMinutes:D2}:{remaining.Seconds:D2}";
            ProgressValue = 1 - (remaining.TotalSeconds / (FocusDuration * 60));
        });
    }

    private async void OnTimerCompleted(object? sender, FocusSession session)
    {
        await _databaseService.SaveSessionAsync(session);
        
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            await _notificationService.ShowNotificationAsync(
                "Focus Session Complete! ??",
                $"Great job! You completed a {session.DurationMinutes} minute focus session.");

            ResetTimer();
        });
    }

    private void OnTimerStarted(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            IsRunning = true;
            IsPaused = false;
            CanModifyDuration = false;
            StartButtonText = "Pause";
        });
    }

    private void OnTimerPaused(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            IsPaused = true;
            StartButtonText = "Resume";
        });
    }

    private void OnTimerResumed(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            IsPaused = false;
            StartButtonText = "Pause";
        });
    }

    private void ResetTimer()
    {
        IsRunning = false;
        IsPaused = false;
        CanModifyDuration = true;
        StartButtonText = "Start";
        ProgressValue = 0;
        UpdateDisplayTime();
    }

    private void UpdateDisplayTime()
    {
        DisplayTime = $"{FocusDuration:D2}:00";
    }
}

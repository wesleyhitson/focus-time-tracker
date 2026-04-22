using System.Timers;
using FocusTimeTracker.Models;

namespace FocusTimeTracker.Services;

public class TimerService
{
    private System.Timers.Timer? _timer;
    private DateTime _startTime;
    private TimeSpan _targetDuration;
    private TimeSpan _elapsed;
    private FocusSession? _currentSession;

    public bool IsRunning { get; private set; }
    public bool IsPaused { get; private set; }
    public TimeSpan Elapsed => _elapsed;
    public TimeSpan Remaining => _targetDuration - _elapsed;
    public FocusSession? CurrentSession => _currentSession;

    public event EventHandler<TimeSpan>? Tick;
    public event EventHandler<FocusSession>? Completed;
    public event EventHandler? Started;
    public event EventHandler? Paused;
    public event EventHandler? Resumed;

    public void Start(int durationMinutes, string category = "General")
    {
        if (IsRunning)
            return;

        _targetDuration = TimeSpan.FromMinutes(durationMinutes);
        _startTime = DateTime.Now;
        _elapsed = TimeSpan.Zero;
        IsRunning = true;
        IsPaused = false;

        _currentSession = new FocusSession
        {
            StartTime = _startTime,
            DurationMinutes = durationMinutes,
            Category = category,
            Completed = false
        };

        _timer = new System.Timers.Timer(1000); // 1 second interval
        _timer.Elapsed += OnTimerTick;
        _timer.Start();

        Started?.Invoke(this, EventArgs.Empty);
    }

    public void Pause()
    {
        if (!IsRunning || IsPaused)
            return;

        _timer?.Stop();
        IsPaused = true;
        Paused?.Invoke(this, EventArgs.Empty);
    }

    public void Resume()
    {
        if (!IsRunning || !IsPaused)
            return;

        _startTime = DateTime.Now - _elapsed;
        _timer?.Start();
        IsPaused = false;
        Resumed?.Invoke(this, EventArgs.Empty);
    }

    public FocusSession Stop()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;

        if (_currentSession != null)
        {
            _currentSession.EndTime = DateTime.Now;
            _currentSession.ActualMinutes = (int)_elapsed.TotalMinutes;
            _currentSession.Completed = false;
        }

        IsRunning = false;
        IsPaused = false;

        var session = _currentSession!;
        _currentSession = null;

        return session;
    }

    private void OnTimerTick(object? sender, ElapsedEventArgs e)
    {
        if (IsPaused)
            return;

        // Round to nearest second to prevent drift and skipping
        var actualElapsed = DateTime.Now - _startTime;
        _elapsed = TimeSpan.FromSeconds(Math.Round(actualElapsed.TotalSeconds));

        if (_elapsed >= _targetDuration)
        {
            _elapsed = _targetDuration;
            CompleteSession();
        }
        else
        {
            Tick?.Invoke(this, Remaining);
        }
    }

    private void CompleteSession()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;

        if (_currentSession != null)
        {
            _currentSession.EndTime = DateTime.Now;
            _currentSession.ActualMinutes = _currentSession.DurationMinutes;
            _currentSession.Completed = true;
            
            Completed?.Invoke(this, _currentSession);
        }

        IsRunning = false;
        IsPaused = false;
        var session = _currentSession!;
        _currentSession = null;
    }

    public void Reset()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
        
        IsRunning = false;
        IsPaused = false;
        _elapsed = TimeSpan.Zero;
        _currentSession = null;
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FocusTimeTracker.Models;
using FocusTimeTracker.Services;
using System.Collections.ObjectModel;

namespace FocusTimeTracker.ViewModels;

public partial class StatisticsViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<DailyStatistic> _dailyStats = new();

    [ObservableProperty]
    private int _totalFocusMinutes = 0;

    [ObservableProperty]
    private int _totalSessions = 0;

    [ObservableProperty]
    private int _completedSessions = 0;

    [ObservableProperty]
    private string _totalFocusTime = "0h 0m";

    [ObservableProperty]
    private string _completionRate = "0%";

    [ObservableProperty]
    private int _selectedDays = 7;

    public StatisticsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [RelayCommand]
    private async Task ChangePeriod(object parameter)
    {
        if (parameter is string days && int.TryParse(days, out int numDays))
        {
            SelectedDays = numDays;
            await LoadDailyStatsAsync();
        }
    }

    public async Task LoadStatisticsAsync()
    {
        await LoadDailyStatsAsync();
        await LoadTotalsAsync();
    }

    public async Task LoadDailyStatsAsync()
    {
        var stats = await _databaseService.GetDailyStatisticsAsync(SelectedDays);
        DailyStats.Clear();
        
        foreach (var stat in stats)
        {
            DailyStats.Add(stat);
        }
    }

    private async Task LoadTotalsAsync()
    {
        TotalFocusMinutes = await _databaseService.GetTotalFocusMinutesAsync();
        TotalSessions = await _databaseService.GetTotalSessionsAsync();
        CompletedSessions = await _databaseService.GetCompletedSessionsAsync();

        var hours = TotalFocusMinutes / 60;
        var minutes = TotalFocusMinutes % 60;
        TotalFocusTime = $"{hours}h {minutes}m";

        if (TotalSessions > 0)
        {
            var rate = (double)CompletedSessions / TotalSessions * 100;
            CompletionRate = $"{rate:F0}%";
        }
        else
        {
            CompletionRate = "0%";
        }
    }

    partial void OnSelectedDaysChanged(int value)
    {
        _ = LoadDailyStatsAsync();
    }
}

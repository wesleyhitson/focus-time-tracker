using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FocusTimeTracker.Models;
using FocusTimeTracker.Services;
using System.Collections.ObjectModel;

namespace FocusTimeTracker.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<FocusSession> _sessions = new();

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private bool _hasNoSessions = false;

    public HistoryViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task LoadSessionsAsync()
    {
        IsLoading = true;
        
        try
        {
            var sessions = await _databaseService.GetSessionsAsync();
            Sessions.Clear();
            
            foreach (var session in sessions)
            {
                Sessions.Add(session);
            }

            HasNoSessions = Sessions.Count == 0;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteSession(FocusSession session)
    {
        await _databaseService.DeleteSessionAsync(session);
        Sessions.Remove(session);
        HasNoSessions = Sessions.Count == 0;
    }
}

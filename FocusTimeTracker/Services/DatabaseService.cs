using SQLite;
using FocusTimeTracker.Models;

namespace FocusTimeTracker.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;
    private readonly string _dbPath;

    public DatabaseService()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appDataPath, "FocusTimeTracker");
        Directory.CreateDirectory(appFolder);
        _dbPath = Path.Combine(appFolder, "focus_tracker.db");
    }

    private async Task InitAsync()
    {
        if (_database != null)
            return;

        _database = new SQLiteAsyncConnection(_dbPath);
        await _database.CreateTableAsync<FocusSession>();
    }

    public async Task<List<FocusSession>> GetSessionsAsync()
    {
        await InitAsync();
        return await _database!.Table<FocusSession>()
            .OrderByDescending(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<List<FocusSession>> GetSessionsByDateRangeAsync(DateTime start, DateTime end)
    {
        await InitAsync();
        return await _database!.Table<FocusSession>()
            .Where(s => s.StartTime >= start && s.StartTime <= end)
            .OrderByDescending(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<List<FocusSession>> GetTodaySessionsAsync()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        return await GetSessionsByDateRangeAsync(today, tomorrow);
    }

    public async Task<FocusSession?> GetSessionAsync(int id)
    {
        await InitAsync();
        return await _database!.FindAsync<FocusSession>(id);
    }

    public async Task<int> SaveSessionAsync(FocusSession session)
    {
        await InitAsync();

        if (session.Id != 0)
            return await _database!.UpdateAsync(session);
        else
            return await _database!.InsertAsync(session);
    }

    public async Task<int> DeleteSessionAsync(FocusSession session)
    {
        await InitAsync();
        return await _database!.DeleteAsync(session);
    }

    public async Task<List<DailyStatistic>> GetDailyStatisticsAsync(int days = 7)
    {
        await InitAsync();
        
        var startDate = DateTime.Today.AddDays(-days + 1);
        var sessions = await GetSessionsByDateRangeAsync(startDate, DateTime.Now);

        var stats = sessions
            .GroupBy(s => s.StartTime.Date)
            .Select(g => new DailyStatistic
            {
                Date = g.Key,
                TotalMinutes = g.Sum(s => s.Completed ? s.DurationMinutes : s.ActualMinutes),
                CompletedSessions = g.Count(s => s.Completed),
                TotalSessions = g.Count()
            })
            .OrderBy(s => s.Date)
            .ToList();

        // Fill in missing days with zero stats
        var allDays = new List<DailyStatistic>();
        for (int i = 0; i < days; i++)
        {
            var date = startDate.AddDays(i);
            var stat = stats.FirstOrDefault(s => s.Date == date);
            allDays.Add(stat ?? new DailyStatistic 
            { 
                Date = date, 
                TotalMinutes = 0, 
                CompletedSessions = 0, 
                TotalSessions = 0 
            });
        }

        return allDays;
    }

    public async Task<int> GetTotalFocusMinutesAsync()
    {
        await InitAsync();
        var sessions = await GetSessionsAsync();
        return sessions.Sum(s => s.Completed ? s.DurationMinutes : s.ActualMinutes);
    }

    public async Task<int> GetTotalSessionsAsync()
    {
        await InitAsync();
        return await _database!.Table<FocusSession>().CountAsync();
    }

    public async Task<int> GetCompletedSessionsAsync()
    {
        await InitAsync();
        return await _database!.Table<FocusSession>()
            .Where(s => s.Completed)
            .CountAsync();
    }
}

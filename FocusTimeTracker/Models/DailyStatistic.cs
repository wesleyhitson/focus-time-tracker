namespace FocusTimeTracker.Models;

public class DailyStatistic
{
    public DateTime Date { get; set; }
    public int TotalMinutes { get; set; }
    public int CompletedSessions { get; set; }
    public int TotalSessions { get; set; }

    public string DisplayDate => Date.ToString("MMM dd");
    public string DisplayHours => $"{TotalMinutes / 60}h {TotalMinutes % 60}m";
    public double CompletionRate => TotalSessions > 0 ? (double)CompletedSessions / TotalSessions * 100 : 0;
}

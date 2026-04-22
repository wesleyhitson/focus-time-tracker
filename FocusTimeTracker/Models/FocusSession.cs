using SQLite;

namespace FocusTimeTracker.Models;

[Table("focus_sessions")]
public class FocusSession
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    [NotNull]
    public int DurationMinutes { get; set; }

    public int ActualMinutes { get; set; }

    [NotNull]
    public string Category { get; set; } = "General";

    public bool Completed { get; set; }

    public string? Notes { get; set; }

    [Ignore]
    public TimeSpan Duration => TimeSpan.FromMinutes(DurationMinutes);

    [Ignore]
    public TimeSpan ActualDuration => TimeSpan.FromMinutes(ActualMinutes);

    [Ignore]
    public string DisplayDate => StartTime.ToString("MMM dd, yyyy");

    [Ignore]
    public string DisplayTime => StartTime.ToString("h:mm tt");

    [Ignore]
    public string StatusIcon => Completed ? "\u2713" : "\u2715";

    [Ignore]
    public string StatusColor => Completed ? "#4CAF50" : "#FF9800";
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FocusTimeTracker.Services;
using System.IO;
using System.Text;
using FocusTimeTracker.Models;

namespace FocusTimeTracker.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private string _exportMessage = "";

    [ObservableProperty]
    private string _importMessage = "";

    public SettingsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [RelayCommand]
    private async Task ExportData()
    {
        try
        {
            var sessions = await _databaseService.GetSessionsAsync();

            if (sessions.Count == 0)
            {
                ExportMessage = "No data to export.";
                return;
            }

            var csv = new StringBuilder();
            csv.AppendLine("StartTime,EndTime,DurationMinutes,ActualMinutes,Category,Completed,Notes");

            foreach (var session in sessions)
            {
                csv.AppendLine($"{session.StartTime:yyyy-MM-dd HH:mm:ss}," +
                              $"{session.EndTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? ""}," +
                              $"{session.DurationMinutes}," +
                              $"{session.ActualMinutes}," +
                              $"\"{session.Category}\"," +
                              $"{session.Completed}," +
                              $"\"{session.Notes ?? ""}\"");
            }

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = $"FocusTimeTracker_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var filePath = Path.Combine(documentsPath, fileName);

            await File.WriteAllTextAsync(filePath, csv.ToString());

            ExportMessage = $"? Exported {sessions.Count} sessions to:\n{filePath}";
        }
        catch (Exception ex)
        {
            ExportMessage = $"? Export failed: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task ImportData()
    {
        try
        {
            ImportMessage = "Please select a CSV file to import...";

            // For now, we'll use a simple file path approach
            // You'll need to manually specify the path or use a file picker
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var potentialFiles = Directory.GetFiles(documentsPath, "FocusTimeTracker_Export_*.csv");

            if (potentialFiles.Length == 0)
            {
                ImportMessage = "? No export files found in Documents folder.";
                return;
            }

            // Import the most recent file
            var filePath = potentialFiles.OrderByDescending(f => f).First();
            var lines = await File.ReadAllLinesAsync(filePath);

            if (lines.Length <= 1)
            {
                ImportMessage = "? CSV file is empty or invalid.";
                return;
            }

            int importedCount = 0;
            for (int i = 1; i < lines.Length; i++)
            {
                var parts = ParseCsvLine(lines[i]);
                if (parts.Length >= 6)
                {
                    var session = new FocusSession
                    {
                        StartTime = DateTime.Parse(parts[0]),
                        EndTime = string.IsNullOrEmpty(parts[1]) ? null : DateTime.Parse(parts[1]),
                        DurationMinutes = int.Parse(parts[2]),
                        ActualMinutes = int.Parse(parts[3]),
                        Category = parts[4].Trim('"'),
                        Completed = bool.Parse(parts[5]),
                        Notes = parts.Length > 6 ? parts[6].Trim('"') : null
                    };

                    await _databaseService.SaveSessionAsync(session);
                    importedCount++;
                }
            }

            ImportMessage = $"? Imported {importedCount} sessions from:\n{Path.GetFileName(filePath)}";
        }
        catch (Exception ex)
        {
            ImportMessage = $"? Import failed: {ex.Message}";
        }
    }

    private string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        var currentField = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(currentField.ToString());
                currentField.Clear();
            }
            else
            {
                currentField.Append(c);
            }
        }

        result.Add(currentField.ToString());
        return result.ToArray();
    }
}

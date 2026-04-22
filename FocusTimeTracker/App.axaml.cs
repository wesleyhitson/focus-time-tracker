using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FocusTimeTracker.Services;
using FocusTimeTracker.ViewModels;
using FocusTimeTracker.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FocusTimeTracker;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Configure services
        var services = new ServiceCollection();

        // Register Services
        services.AddSingleton<DatabaseService>();
        services.AddSingleton<TimerService>();
        services.AddSingleton<INotificationService, NotificationService>();

        // Register ViewModels
        services.AddTransient<TimerViewModel>();
        services.AddTransient<HistoryViewModel>();
        services.AddTransient<StatisticsViewModel>();
        services.AddTransient<SettingsViewModel>();

        // Register Views
        services.AddTransient<TimerView>();
        services.AddTransient<HistoryView>();
        services.AddTransient<StatisticsView>();
        services.AddTransient<SettingsView>();
        services.AddTransient<MainWindow>();

        Services = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}

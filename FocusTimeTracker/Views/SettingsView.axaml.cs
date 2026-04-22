using Avalonia.Controls;
using FocusTimeTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FocusTimeTracker.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<SettingsViewModel>();
    }
}

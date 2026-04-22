using Avalonia.Controls;
using FocusTimeTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FocusTimeTracker.Views;

public partial class TimerView : UserControl
{
    public TimerView()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<TimerViewModel>();
    }
}

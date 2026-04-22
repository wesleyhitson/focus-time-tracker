using Avalonia.Controls;
using FocusTimeTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FocusTimeTracker.Views;

public partial class StatisticsView : UserControl
{
    public StatisticsView()
    {
        InitializeComponent();
        var viewModel = App.Services.GetRequiredService<StatisticsViewModel>();
        DataContext = viewModel;

        // Load data when view is attached
        AttachedToVisualTree += async (s, e) => await viewModel.LoadStatisticsAsync();
    }
}

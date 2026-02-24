using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using StringifyMaintenance.Pages;

namespace StringifyMaintenance;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void Navigate(Page page)
    {
        MainFrame.Navigate(page);
    }

    public void NavigateToLogin()
    {
        Navigate(App.Services.GetRequiredService<LoginPage>());
    }

    public void NavigateToMaintenance()
    {
        Navigate(App.Services.GetRequiredService<MaintenancePage>());
    }
}
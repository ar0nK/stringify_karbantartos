using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using StringifyMaintenance.Services;

namespace StringifyMaintenance.Pages;

public partial class LoginPage : Page
{
    private readonly AuthService _authService;
    private readonly MainWindow _mainWindow;

    public LoginPage()
    {
        InitializeComponent();
        _authService = App.Services.GetRequiredService<AuthService>();
        _mainWindow = (MainWindow)Application.Current.MainWindow;
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        ErrorTextBlock.Text = string.Empty;

        string email = EmailTextBox.Text.Trim();
        string password = PasswordBox.Password;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ErrorTextBlock.Text = "Email and password are required.";
            return;
        }

        try
        {
            var user = await _authService.AuthenticateAsync(email, password);
            if (user == null)
            {
                ErrorTextBlock.Text = "Invalid credentials or inactive user.";
                return;
            }

            Session.CurrentUser = user;
            _mainWindow.NavigateToMaintenance();
        }
        catch (Exception ex)
        {
            ErrorTextBlock.Text = $"Login failed: {ex.Message}";
        }
    }
}

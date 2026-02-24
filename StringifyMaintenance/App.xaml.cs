using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StringifyMaintenance.Data;
using StringifyMaintenance.Pages;
using StringifyMaintenance.Services;

namespace StringifyMaintenance;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private readonly IHost _host;

	public App()
	{
		_host = Host.CreateDefaultBuilder()
			.ConfigureAppConfiguration(builder =>
			{
				builder.SetBasePath(AppContext.BaseDirectory);
				builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
			})
			.ConfigureServices((context, services) =>
			{
				string connectionString = context.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

				services.AddDbContextFactory<StringifyDbContext>(options =>
					options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 5, 0))));

				services.AddSingleton<AuthService>();
				services.AddSingleton<MaintenanceRepository>();

				services.AddSingleton<MainWindow>();
				services.AddTransient<LoginPage>();
				services.AddTransient<MaintenancePage>();
			})
			.Build();
	}

	public static IServiceProvider Services => ((App)Current)._host.Services;

	protected override async void OnStartup(StartupEventArgs e)
	{
		await _host.StartAsync();

		var mainWindow = Services.GetRequiredService<MainWindow>();
		mainWindow.Show();
		mainWindow.NavigateToLogin();

		base.OnStartup(e);
	}

	protected override async void OnExit(ExitEventArgs e)
	{
		await _host.StopAsync();
		_host.Dispose();
		base.OnExit(e);
	}
}


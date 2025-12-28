using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;

namespace lab013;

class Program
{
	// Инициализация Avalonia перед запуском приложения
	[STAThread]
	public static void Main(string[] args) => BuildAvaloniaApp()
		.StartWithClassicDesktopLifetime(args);

	// Конфигурация Avalonia приложения
	public static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.WithInterFont()
			.LogToTrace();
}

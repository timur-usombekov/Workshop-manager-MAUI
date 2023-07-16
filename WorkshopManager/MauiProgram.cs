using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkshopManager.ViewModels;
using WorkshopManager.Views;

namespace WorkshopManager;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<EmployeeViewModel>();
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<AddNewEmployeePage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

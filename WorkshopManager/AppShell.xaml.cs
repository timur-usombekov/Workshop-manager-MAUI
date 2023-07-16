using WorkshopManager.Views;

namespace WorkshopManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(AddNewEmployeePage), typeof(AddNewEmployeePage));
	}
}

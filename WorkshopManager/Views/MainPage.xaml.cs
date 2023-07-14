using WorkshopManager.ViewModels;

namespace WorkshopManager.Views;

public partial class MainPage : ContentPage
{
	public MainPage(EmployeeViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}


using WorkshopManager.Models;
using WorkshopManager.ViewModels;

namespace WorkshopManager.Views;

public partial class EmployeeDetailsPage : ContentPage
{
	public EmployeeDetailsPage(EmployeeDetailsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
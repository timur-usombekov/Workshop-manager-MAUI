using WorkshopManager.ViewModels;

namespace WorkshopManager.Views;

public partial class AddNewEmployeePage : ContentPage
{
	public AddNewEmployeePage(EmployeeViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

    }
}
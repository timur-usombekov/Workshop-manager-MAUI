using WorkshopManager.ViewModels;

namespace WorkshopManager.Views;

public partial class AddNewEmployeePage : ContentPage
{
	public AddNewEmployeePage(AddNewEmployeeViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;


    }
}
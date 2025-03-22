using TradeCompApp.Models;
using TradeCompApp.ViewModels;

namespace TradeCompApp;

public partial class CategoriesPage : ContentPage
{
	public CategoriesPage()
	{
		InitializeComponent();
        BindingContext = new CategoryViewModel();
    }

    private async void ProductCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        //await Navigation.PushAsync(new Cataloge());

    }
    private void OnSelectClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
    }
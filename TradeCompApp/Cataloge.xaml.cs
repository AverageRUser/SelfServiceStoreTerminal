using System.Collections.ObjectModel;
using TradeCompApp.Models;
using TradeCompApp.ViewModels;


namespace TradeCompApp;

public partial class Cataloge : ContentPage
{

    public Cataloge()
	{
		InitializeComponent();
        BindingContext = new CatalogeViewModel();
    }

    private async void OnGoToCartClicked(object sender, EventArgs e)
    {


        await Navigation.PushAsync(new Cart());
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnProductSelected(object sender, SelectedItemChangedEventArgs e)
    {
       
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {

    }
}
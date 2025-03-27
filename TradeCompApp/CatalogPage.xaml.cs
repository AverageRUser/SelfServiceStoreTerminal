using System.Collections.ObjectModel;
using TradeCompApp.Models;
using TradeCompApp.ViewModels;


namespace TradeCompApp;

public partial class CatalogPage : ContentPage
{

    public CatalogPage()
	{
		InitializeComponent();
        BindingContext = new CatalogViewModel();
    }

    private async void OnGoToCartClicked(object sender, EventArgs e)
    {


        await Navigation.PushAsync(new CartPage());
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

  

    private async void OnSelectCategoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CategoriesPage());
    }
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private async void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await Navigation.PushAsync(new CartPage());
    }
}
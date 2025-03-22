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

    private async void OnProductSelected(object sender, SelectedItemChangedEventArgs e)
    {
       
    }

    private async void OnSelectCategoryClicked(object sender, EventArgs e)
    {
        var categorySelectionPage = new CategoriesPage();
      
        await Navigation.PushAsync(categorySelectionPage);
    }
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {

    }
}
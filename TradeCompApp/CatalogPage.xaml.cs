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

    private async void FilterPanel_Open(object sender, EventArgs e)
    {

        if (Math.Abs(FiltersPanel.TranslationX) > 0)
        {
            // Открываем
            FiltersPanel.IsVisible = true;
            await FiltersPanel.TranslateTo(0, 0, 300, Easing.SinOut);
        }
        else
        {
            // Закрываем
            await FiltersPanel.TranslateTo(-315, 0, 300, Easing.SinIn);
            FiltersPanel.IsVisible = false;
        }

    }


    private async void FilterPanel_Close(object sender, EventArgs e)
    {

        await FiltersPanel.TranslateTo(-315, 0, 300, Easing.SinIn);
        FiltersPanel.IsVisible = false;
    }
}
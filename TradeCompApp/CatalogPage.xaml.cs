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

    private async void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await Navigation.PushAsync(new CartPage());
    }
    private void FilterPanel_Open(object sender, EventArgs e)
    {

        if (Math.Abs(FiltersPanel.TranslationX) > 0)
        {
            // Открываем
            FiltersPanel.TranslateTo(0, 0, 250, Easing.CubicOut);
            FiltersPanel.IsVisible = true;
        }
        else
        {
            // Закрываем
            FiltersPanel.TranslateTo(-FiltersPanel.Width, 0, 250, Easing.CubicIn);
          FiltersPanel.IsVisible = false;
        }

    }


    private void FilterPanel_Close(object sender, EventArgs e)
    {
       
        FiltersPanel.TranslateTo(-FiltersPanel.Width, 0, 250, Easing.CubicIn);
        FiltersPanel.IsVisible = false;
    }
}
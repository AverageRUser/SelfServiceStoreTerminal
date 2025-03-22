using TradeCompApp.Models;
using TradeCompApp.ViewModels;

namespace TradeCompApp;

public partial class Cart : ContentPage
{
  
	public Cart()
	{
		InitializeComponent();
        BindingContext = CartViewModel.Instance;
    }

    private async void OnProceedToPaymentClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaymentPage());
    }

    private async void OnContinueShoppingClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnAddManualProductClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CatalogPage());
    }
   

}
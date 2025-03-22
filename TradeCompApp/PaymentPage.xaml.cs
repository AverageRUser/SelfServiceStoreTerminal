using TradeCompApp.ViewModels;

namespace TradeCompApp;

public partial class PaymentPage : ContentPage
{
	public PaymentPage()
	{
		InitializeComponent();
        BindingContext = CartViewModel.Instance;
    }

    private async void OnPayClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CompletionPage());
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
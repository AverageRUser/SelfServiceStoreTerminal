using TradeCompApp.ViewModels;

namespace TradeCompApp;

public partial class CompletionPage : ContentPage
{
	public CompletionPage()
	{
		InitializeComponent();
        BindingContext = CartViewModel.Instance;
    }

    private void OnPrintReceiptClicked(object sender, EventArgs e)
    {

    }

    private void OnEmailReceiptClicked(object sender, EventArgs e)
    {

    }

   
}
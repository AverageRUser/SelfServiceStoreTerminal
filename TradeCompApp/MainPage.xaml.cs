using Google.Apis.Auth.OAuth2;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Devices;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace TradeCompApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new CatalogPage());
           
        }

    

        private async void OnHelpClicked(object sender, EventArgs e)
        {       
          
            await Navigation.PushAsync(new HelpPage());
        }
    }

}

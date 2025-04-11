using Google.Apis.Auth.OAuth2;

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
         
#if WINDOWS || MACCATALYST
            var window = GetParentWindow();
            if (window != null)
            {
                window.Width = 1200; 
                window.Height = 950;
            }
#endif
          
            await Navigation.PushAsync(new CatalogPage());
           
        }

    

        private async void OnHelpClicked(object sender, EventArgs e)
        {       
          
            await Navigation.PushAsync(new HelpPage());
        }
    }

}

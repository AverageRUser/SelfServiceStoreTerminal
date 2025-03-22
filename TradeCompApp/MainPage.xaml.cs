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

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
          
        }

        private async void OnHelpClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HelpPage());
        }
    }

}

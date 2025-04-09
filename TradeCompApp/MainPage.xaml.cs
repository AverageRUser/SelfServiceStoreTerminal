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
                window.Width = 1200; // Ширина окна
                window.Height = 950; // Высота окна
            }
#endif
            await Navigation.PushAsync(new CatalogPage());
           
        }

    

        private async void OnHelpClicked(object sender, EventArgs e)
        {       
            //await SecureStorage.SetAsync("mysql_connection", connectionstring);
            await Navigation.PushAsync(new HelpPage());
        }
    }

}

using TradeCompApp.ViewModels;

namespace TradeCompApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        protected override async void OnStart()
        {
            await CartViewModel.Instance.InitializeSevices();
        }
    }
}

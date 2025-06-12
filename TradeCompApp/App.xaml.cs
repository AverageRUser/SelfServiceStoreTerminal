using TradeCompApp.Database;
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
            await DatabaseStatus.CheckConnectionAsync();
            await CartViewModel.Instance.InitializeSevices();
        }
    }
}

using Google.Apis.Util;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using TradeCompApp.Database;
using TradeCompApp.Models;





namespace TradeCompApp.ViewModels
{
    class CartViewModel : INotifyPropertyChanged
    {
        
        private static CartViewModel _instance;
        private Dictionary<int, ObservableCollection<ProductService>> _categoryServices;
        
       
        private bool _isEnabled;
        private string _selectedDelivery;
        private string _selectedPayment;
        private string _receiptText;
        private string _emailtext;

        public static CartViewModel Instance => _instance ??= new CartViewModel();
        public ICommand RemoveCommand { get; set; }
        public ICommand AddQuantityCommand { get; set; }
        public ICommand DistQuantityCommand { get; set; }
        public ICommand ServiceCheckedCommand => new Command<ProductService>(OnServiceCheckedChanged);
        public ICommand OnPrintReceiptCommand => new Command(OnPrintReceipt);
        public ICommand OnBackToMainPageCommand => new Command(OnBackToMainPage);

       
        public ICommand OnPrintEmailReceiptCommand => new Command(OnEmailPrintReceipt);

        private readonly DatabaseService _databaseService;
        private readonly GoogleAuthService _AuthService;
        public ObservableCollection<CartItem> CartProduction { get; } = new();
       
        public decimal TotalPrice => CartProduction.Sum(item => item.TotalPrice);
        public string EmailText
        {
            get => _emailtext;
            set
            {
                _emailtext = value;
                OnPropertyChanged(nameof(EmailText));
            }
        }
        public bool ButtonIsEnabled
        {
            get => _isEnabled;
            set   
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SelectedDelivery
        {
            get => _selectedDelivery;
            set
            {
                _selectedDelivery = value;
                OnPropertyChanged();
            }
        }
        public string SelectedPayment
        {
            get => _selectedPayment;
            set
            {
                _selectedPayment = value;
                OnPropertyChanged();
            }
        }
        public string ReceiptText
        {
            get => _receiptText;
            set
            {
                _receiptText = value;
                OnPropertyChanged();
            }
        }
        public CartViewModel()
        {
            _databaseService = new DatabaseService();
            _AuthService = new GoogleAuthService();
            //InitializeSevices();
          
            RemoveCommand = new Command<CartItem>((CartItem item) =>
            {
                CartProduction.Remove(item);

                OnPropertyChanged(nameof(TotalPrice));
            });
            AddQuantityCommand = new Command<CartItem>((CartItem item) =>
            {
                if (item != null)
                {
                    item.Quantity++;
                }
                OnPropertyChanged(nameof(TotalPrice));
            });
            DistQuantityCommand = new Command<CartItem>((CartItem item) =>
            {
                if (item != null)
                {
                    if (item.Quantity != 1)
                    {
                        item.Quantity--;
                        
                    }
                    else
                    {
                        ButtonIsEnabled = false;
                    }
                }
                OnPropertyChanged(nameof(TotalPrice));
            });
           
        }
        private void OnServiceCheckedChanged(ProductService service)
        {
            // Находим родительский CartItem, которому принадлежит услуга
            var cartItem = CartProduction.FirstOrDefault(item => item.Services.Contains(service));
            cartItem?.OnPropertyChanged(nameof(CartItem.TotalPrice));

            OnPropertyChanged(nameof(TotalPrice));
        }
        public async Task InitializeSevices()
        {
            try
            {
                _categoryServices = new Dictionary<int, ObservableCollection<ProductService>>();
                var categories = await _databaseService.GetAllCategories();

                foreach (var category in categories)
                {
                    // Загружаем сервисы для каждой категории
                    var services = await _databaseService.GetProductServices(category.Id);
                    _categoryServices[category.Id] = new ObservableCollection<ProductService>(services);
                }
            }
            catch (Exception ex)
            {
                
                //Загрузка данныпо умолчанию, если не удалось подключиться к СУБД
                _categoryServices = new Dictionary<int, ObservableCollection<ProductService>>
                {
                    [1] = new ObservableCollection<ProductService>
                    {
                        new ProductService { Name = "Установка телевизора", Price = 1500, CategoryId = 1 },
                        new ProductService { Name = "Настройка телевизора", Price = 800, CategoryId = 1 }
                    },
                    [4] = new ObservableCollection<ProductService>
                    {
                        new ProductService { Name = "Установка ОС", Price = 2000, CategoryId = 4},
                        new ProductService { Name = "Настройка программ", Price = 1000, CategoryId = 4 }
                    },
                    [2] = new ObservableCollection<ProductService>
                    {
                        new ProductService { Name = "Перенос данных", Price = 500, CategoryId = 2},
                        new ProductService { Name = "Установка защитного стекла", Price = 300, CategoryId = 2}
                    },
                    [3] = new ObservableCollection<ProductService>
                    {
                        new ProductService { Name = "Установка техники", Price = 1000, CategoryId = 3 },
                        new ProductService { Name = "Демонтаж старой техники", Price = 500, CategoryId = 3 }
                    }
                };
                
            }
        }
        private async void OnBackToMainPage()
        {
            CartProduction.Clear();
            if (Shell.Current != null)
            {
                await Shell.Current.Navigation.PopToRootAsync();
            }
        }

        public async void OnPrintReceipt()
        {
            Receipt receipt = await _databaseService.GetReceipt();
        
            //Для отладки
            ReceiptText = ReceiptBuilder(receipt).ToString();

            Shell.Current.DisplayAlert("Чек", "Успешно отправленно!", "Вернуться на главную");
                OnBackToMainPage();
            

        }
        public StringBuilder ReceiptBuilder(Receipt receipt)
        {
            var receiptBuilder = new StringBuilder();

            receiptBuilder.AppendLine($"ООО Успех");
            receiptBuilder.AppendLine($"Добро пожаловать!");
            receiptBuilder.AppendLine($"ИНН {receipt.INN}");
            receiptBuilder.AppendLine($"ККМ {receipt.KKM}");
            receiptBuilder.AppendLine($"ЭКЛЗ {receipt.EKLZ}");
            receiptBuilder.AppendLine($"Адрес - {receipt.Address}");
            receiptBuilder.AppendLine($"{receipt.CreatedAt}");
            receiptBuilder.AppendLine($"{receipt.Position}");
            receiptBuilder.AppendLine($"{receipt.FIO}");
            receiptBuilder.AppendLine($"Товары:");
            foreach (var item in CartProduction)
            {
                receiptBuilder.AppendLine($"{item.Product.Name} \n {item.Quantity} \t = {item.Product.Price:C} ");
            }
            receiptBuilder.AppendLine($"Услуги:");
            foreach (var item in CartProduction)
            {
                foreach (var service in item.Services)
                {
                    if (service.IsSelectedService == true)
                        receiptBuilder.AppendLine($"{service.Name} \n\t  = {service.Price:C} ");
                }

            }
            receiptBuilder.AppendLine($"---------------------------");
            receiptBuilder.AppendLine($"Итого: {TotalPrice:C}");
            receiptBuilder.AppendLine($"Способ оплаты: {SelectedPayment}");
            receiptBuilder.AppendLine($"Способ доставки: {SelectedDelivery}");
            receiptBuilder.AppendLine($"---------------------------");
            receiptBuilder.AppendLine($"Чек №{receipt.Id}");
            return receiptBuilder;
        }
        public async void OnEmailPrintReceipt()
        {
            Receipt receipt = await _databaseService.GetReceipt();

       
            await SendReceiptByEmail(EmailText, "Электронный Чек", ReceiptBuilder(receipt).ToString());
        }
      

        public async Task SendReceiptByEmail( string customerEmail, string subject,string body)
        {
            try
            {
                var oauthToken = await _AuthService.GetOAuthTokenAsync();
                //string password = await SecureStorage.GetAsync("accountsmpt");
              
                var email = new MimeMessage();
              
                email.From.Add(new MailboxAddress("ООО Успех", "uspehru231@gmail.com"));
                email.To.Add(new MailboxAddress("Recipient", customerEmail));
                email.Subject = subject;

                email.Body = new TextPart("plain") { Text = body };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                var oauth2 = new SaslMechanismOAuth2("uspehru231@gmail.com", oauthToken);
                await smtp.AuthenticateAsync(oauth2);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                Shell.Current.DisplayAlert("Электронный чек", "Успешно отправленно!", "Вернуться на главную");
                OnBackToMainPage();
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Ошибка","Терминал отключен от сети", "Ок");
            }
            
         
            
        }
        public void AddToCart(CartItem item)
        {
            
            var existingItem = CartProduction.FirstOrDefault(i => i.Product.Name == item.Product.Name);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                if (_categoryServices.TryGetValue(item.Product.CategoryId, out var services))
                {
                    item.Services = new ObservableCollection<ProductService>(services);

                }

                CartProduction.Add(item);
            }

            OnPropertyChanged(nameof(TotalPrice));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

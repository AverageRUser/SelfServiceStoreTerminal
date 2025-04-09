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
    

        public static CartViewModel Instance => _instance ??= new CartViewModel();
        public ICommand RemoveCommand { get; set; }
        public ICommand AddQuantityCommand { get; set; }
        public ICommand DistQuantityCommand { get; set; }
        public ICommand ServiceCheckedCommand => new Command<ProductService>(OnServiceCheckedChanged);
        public ICommand OnPrintReceiptCommand => new Command(OnPrintReceipt);


        private readonly DatabaseService _databaseService;
        public ObservableCollection<CartItem> CartProduction { get; } = new();
       
        public decimal TotalPrice => CartProduction.Sum(item => item.TotalPrice);
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
        public async void OnPrintReceipt()
        {
           
            var builder = new StringBuilder();
          
            builder.AppendLine($"ООО Успех");
            builder.AppendLine($"Добро пожаловать!");
            builder.AppendLine($"------------");
            builder.AppendLine($"Товары:");
            foreach (var item in CartProduction)
            {
                builder.AppendLine($"{item.Product.Name} - {item.Product.Price:C} - {item.Quantity} ");
            }
            builder.AppendLine($"Услуги:");
            foreach (var item in CartProduction)
            {
                foreach (var service in item.Services)
                {
                    if(service.IsSelectedService == true)
                    builder.AppendLine($"{service.Name} - {service.Price:C} ");
                }
                
            }
            builder.AppendLine($"------------");
            builder.AppendLine($"Итого: {TotalPrice:C}");
            builder.AppendLine($"Способ оплаты: {SelectedPayment}");
            builder.AppendLine($"Способ доставки: {SelectedDelivery}");
            ReceiptText = builder.ToString();
            await SendReceiptByEmail("amprograms22@gmail.com", "Чек №", builder.ToString());
        }
     
        public async Task SendReceiptByEmail( string customerEmail, string subject,string body)
        {
            /*
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Sender Name", "sender@example.com"));
            email.To.Add(new MailboxAddress("Recipient", customerEmail));
            email.Subject = subject;

            email.Body = new TextPart("plain") { Text = body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("your-email@gmail.com", "your-password");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            */

            /*
             var message = new EmailMessage
             {
                 Subject = $"Чек №",
                 Body = "Test",
                 To = new List<string> { customerEmail }
             };

             await Email.Default.ComposeAsync(message);
            */
        }
        public void AddToCart(CartItem item)
        {
            
            var existingItem = CartProduction.FirstOrDefault(i => i.Product.Name == item.Product.Name);
            
            if (existingItem != null)
                existingItem.Quantity += item.Quantity;
            else
            
            if (_categoryServices.TryGetValue(item.Product.CategoryId, out var services))
            {
                item.Services = new ObservableCollection<ProductService>(services);
                
            }

            CartProduction.Add(item);


            OnPropertyChanged(nameof(TotalPrice));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}



using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using TradeCompApp.Models;

namespace TradeCompApp.ViewModels
{
    class CartViewModel : INotifyPropertyChanged
    {
        
        private static CartViewModel _instance;
        private Dictionary<string, ObservableCollection<ProductService>> _categoryServices;
       
        private bool _isEnabled;
        
        public static CartViewModel Instance => _instance ??= new CartViewModel();
        public ICommand RemoveCommand { get; set; }
        public ICommand AddQuantityCommand { get; set; }
        public ICommand DistQuantityCommand { get; set; }
        public ICommand ServiceCheckedCommand => new Command<ProductService>(OnServiceCheckedChanged);
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
    

        public CartViewModel()
        {

            InitializeSevices();
          
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
        private void InitializeSevices()
        {
            _categoryServices = new Dictionary<string, ObservableCollection<ProductService>>
            {
                ["TV"] = new ObservableCollection<ProductService>
                {
                    new ProductService { Name = "Установка телевизора", Price = 1500, Category = "TV" },
                    new ProductService { Name = "Настройка телевизора", Price = 800, Category = "TV" }
                },
                ["Laptops"] = new ObservableCollection<ProductService>
                {
                    new ProductService { Name = "Установка ОС", Price = 2000, Category = "Laptops" },
                    new ProductService { Name = "Настройка программ", Price = 1000, Category = "Laptops" }
                },
                ["Smartphones"] = new ObservableCollection<ProductService>
                {
                    new ProductService { Name = "Перенос данных", Price = 500, Category = "Smartphones" },
                    new ProductService { Name = "Установка защитного стекла", Price = 300, Category = "Smartphones" }
                },
                ["Appliances"] = new ObservableCollection<ProductService>
                {
                    new ProductService { Name = "Установка техники", Price = 1000, Category = "Appliances" },
                    new ProductService { Name = "Демонтаж старой техники", Price = 500, Category = "Appliances" }
                }
            };
        }
            
            
        public void AddToCart(CartItem item)
        {
            
            var existingItem = CartProduction.FirstOrDefault(i => i.Product.Name == item.Product.Name);
            
            if (existingItem != null)
                existingItem.Quantity += item.Quantity;
            else
                 if (_categoryServices.TryGetValue(item.Product.Type, out var services))
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

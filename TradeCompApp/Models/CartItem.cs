using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TradeCompApp.Models
{
    class CartItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<ProductService> _services;
        private int _quantity;
        public Product Product { get; set; }
        public ProductService ProductService { get; set; }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }
        public ObservableCollection<ProductService> Services
        {
            get => _services;
            set
            {
                if (_services != value)
                {
                    if (_services != null)
                    {
                        foreach (var service in _services)
                        {
                            service.PropertyChanged -= OnServicePropertyChanged;
                        }
                    }

                    _services = value;

                    if (_services != null)
                    {
                        foreach (var service in _services)
                        {
                            service.PropertyChanged += OnServicePropertyChanged;
                        }
                    }

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }
           public decimal TotalPrice
        {
            get
            {
                decimal productPrice = Product.Price * Quantity;
                decimal servicesPrice = Services?
                    .Where(s => s.IsSectetedService)
                    .Sum(s => s.Price) ?? 0;
                return productPrice + servicesPrice;
            }
        }

        private void OnServicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProductService.IsSectetedService) ||
                e.PropertyName == nameof(ProductService.Price))
            {
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

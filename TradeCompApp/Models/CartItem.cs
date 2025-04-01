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
        public decimal TotalPrice => (Product.Price * Quantity) + (Services?.Where(s => s.IsSelectedService).Sum(s => s.Price) ?? 0);
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
                   

                    _services = value;

                 

                    OnPropertyChanged();
                }
            }
        }

     
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

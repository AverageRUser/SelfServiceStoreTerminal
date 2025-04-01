using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TradeCompApp.ViewModels;

namespace TradeCompApp.Models
{
    public class ProductService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _name;
        private decimal _price;
        private bool _isSelectedService;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsSelectedService
        {
            get => _isSelectedService;
            set
            {
                if (_isSelectedService != value)
                {
                    _isSelectedService = value;


                    OnPropertyChanged();
                  CartViewModel.Instance.OnPropertyChanged(nameof(CartViewModel.TotalPrice));
                }
            }
        }
        public string? Category { get; set; }
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}



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
        public static CartViewModel Instance => _instance ??= new CartViewModel();
        public ICommand RemoveCommand { get; set; }
        public ObservableCollection<CartItem> CartProduction { get; } = new();
        public decimal TotalPrice => CartProduction.Sum(item => item.TotalPrice);
        public CartViewModel()
        {

           
          
            RemoveCommand = new Command<CartItem>((CartItem item) =>
            {
                CartProduction.Remove(item);
                OnPropertyChanged(nameof(TotalPrice));
            });
        }
        public void AddToCart(CartItem item)
        {
            /*
            var existingItem = CartProduction.FirstOrDefault(i => i.Product.Id == item.Product.Id);
            
            if (existingItem != null)
                existingItem.Quantity += item.Quantity;
            else
            */
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

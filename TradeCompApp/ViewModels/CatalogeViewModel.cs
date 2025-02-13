
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeCompApp.Models;

namespace TradeCompApp.ViewModels
{
    class CatalogeViewModel : INotifyPropertyChanged
    {
        public ICommand AddToCartCommand => new Command<Products>(AddToCart);
        public ObservableCollection<Products> CatalogeProduction { get; set; } = new()
        {
            new() { Id = 1,Name = "Test", Price = 12, ImageUrl = "dotnet_bot.png", Type = "Fridge" },
            new() { Id = 2,Name = "Fiils", Price = 122, ImageUrl = "dotnet_bot.png", Type = "Cringe" }
        };
        public CatalogeViewModel()
        {
         
        }
        private void AddToCart(Products product)
        {
            // Добавление товара в корзину

           
            CartViewModel.Instance.AddToCart(new CartItem { Product = product, Quantity = 1 });
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

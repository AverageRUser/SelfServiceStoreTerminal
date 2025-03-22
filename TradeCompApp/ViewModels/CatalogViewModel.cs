
using CommunityToolkit.Mvvm.Messaging;
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
    class CatalogViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Products> _products;
        private ObservableCollection<Products> _filteredProducts;
        private string _selectedCategory;
        public ICommand AddToCartCommand => new Command<Products>(AddToCart);
        public ObservableCollection<Products> AllProducts
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Products> FilteredProducts
        {
            get => _filteredProducts;
            set
            {
                _filteredProducts = value;
                OnPropertyChanged();
            }
        }
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                FilterProductsByCategory();
                OnPropertyChanged();
            }
        }
        public CatalogViewModel()
        {
            LoadProducts();
            FilteredProducts = new ObservableCollection<Products>(AllProducts);
            MessagingCenter.Subscribe<CategoryViewModel, string>(this, "CategorySelected", (sender, categoryId) =>
            {
                SelectedCategory = categoryId;
            });
        }
        public void FilterProductsByCategory()
        {
            if (string.IsNullOrEmpty(SelectedCategory))
            {
                FilteredProducts = new ObservableCollection<Products>(AllProducts);
            }
            else
            {
                var filtered = AllProducts.Where(p => p.Type == SelectedCategory).ToList();
                FilteredProducts = new ObservableCollection<Products>(filtered);
            }
        }
        public void LoadProducts()
        {
            
            AllProducts = new ObservableCollection<Products>
            {
                new Products { Name = "Телевизор Samsung 4K", Price = 50000, ImageUrl = "dotnet_bot.png", Type = "TV" },
                new Products { Name = "Телевизор LG OLED", Price = 70000, ImageUrl = "dotnet_bot.png", Type = "TV" },
                new Products { Name = "Ноутбук ASUS ROG", Price = 90000, ImageUrl = "dotnet_bot.png", Type = "Laptops" },
                new Products { Name = "Ноутбук MacBook Pro", Price = 120000, ImageUrl = "dotnet_bot.png", Type = "Laptops" },
                new Products { Name = "Смартфон iPhone 14", Price = 80000, ImageUrl = "dotnet_bot.png", Type = "Smartphones" },
                new Products { Name = "Смартфон Samsung Galaxy", Price = 60000, ImageUrl = "dotnet_bot.png", Type = "Smartphones" },
                new Products { Name = "Холодильник Bosch", Price = 60000, ImageUrl = "dotnet_bot.png", Type= "Appliances" },
                new Products { Name = "Стиральная машина LG", Price = 40000, ImageUrl = "dotnet_bot.png", Type = "Appliances" }
            };

            // Изначально отображаем все товары
            FilteredProducts = new ObservableCollection<Products>(AllProducts);

        }
        public void FilterProductsByCategory(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                // Если категория не выбрана, показываем все товары
                FilteredProducts = new ObservableCollection<Products>(AllProducts);
            }
            else
            {
                // Фильтруем товары по выбранной категории
                var filtered = AllProducts.Where(p => p.Type == categoryId).ToList();
                FilteredProducts = new ObservableCollection<Products>(filtered);
            }
        }
        private void AddToCart(Products product)
        {
           
            CartViewModel.Instance.AddToCart(new CartItem { Product = product, Quantity = 1 });
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

﻿
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
        private ObservableCollection<Product> _products;
        private ObservableCollection<Product> _filteredProducts;
        private ObservableCollection<FilterOption> _filters;
        private bool _visibilityfilter;
        private string _selectedCategory;
        private string _searchText;
        private Product _selectedProduct;
        public ICommand AddToCartCommand => new Command<Product>(AddToCart);
        public ICommand ResetFilterCommand => new Command(OnResetFilter);
        public ICommand SearchCommand => new Command(ExecuteSearch);
        public ICommand ApplyFiltersCommand => new Command(() =>
        {
            var filtered = AllProducts.AsEnumerable();

            foreach (var filter in Filters.Where(f => !string.IsNullOrEmpty(f.SelectedValue)))
            {
                filtered = filtered.Where(p =>
                    p.Specs.Any(s =>
                        s.Name == filter.Name &&
                        s.Value == filter.SelectedValue));
            }

            FilteredProducts = new ObservableCollection<Product>(filtered);
        });
        public ObservableCollection<Product> AllProducts
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<FilterOption> Filters
        {
            get => _filters;
            set
            {
                _filters = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Product> FilteredProducts
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
                VisibilityFilter = !string.IsNullOrEmpty(value);
                FilterProductsByCategory();
                InitializeFilters();
                OnPropertyChanged();
            }
        }
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    ExecuteSearch();
                }
            }
        }


        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                
                OnPropertyChanged();
            }
        }
        public bool VisibilityFilter
        {
            get => _visibilityfilter;
            set
            {
                _visibilityfilter = value;
                OnPropertyChanged();
            }
        }
        public CatalogViewModel()
        {
           
            LoadProducts();
            FilteredProducts = new ObservableCollection<Product>(AllProducts);
            MessagingCenter.Subscribe<CategoryViewModel, string>(this, "CategorySelected", (sender, categoryId) =>
            {
                SelectedCategory = categoryId;
            });
        }
        private void ExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredProducts = new ObservableCollection<Product>(AllProducts);
            }
            else
            {
                var filtered = AllProducts.Where(item => item.Name.Contains(SearchText,StringComparison.OrdinalIgnoreCase)).ToList();
                FilteredProducts = new ObservableCollection<Product>(filtered);
            }
        }
        public void FilterProductsByCategory()
        {
            if (string.IsNullOrEmpty(SelectedCategory))
            {
                FilteredProducts = new ObservableCollection<Product>(AllProducts);
            }
            else
            {
                var filtered = AllProducts.Where(p => p.Type == SelectedCategory).ToList();
                FilteredProducts = new ObservableCollection<Product>(filtered);
            }
           
        }

        private void InitializeFilters()
        {
            Filters = new ObservableCollection<FilterOption>();

         

            var allSpecs = FilteredProducts.SelectMany(p => p.Specs);
            var specGroups = allSpecs.GroupBy(s => s.Name);

            foreach (var group in specGroups)
            {
                Filters.Add(new FilterOption
                {
                    Name = group.Key,
                    AvailableValues = group.Select(s => s.Value).Distinct().ToList()
                });
            }
        }
       
        public void LoadProducts()
        {
            
            AllProducts = new ObservableCollection<Product>
            {
                  new Product { Name = "Телевизор Samsung 4K", Price = 50000, ImageUrl = "dotnet_bot.png", Type = "TV", Specs = new List<TechSpec>{
        new TechSpec { Name = "Диагональ", Value = "55", Unit = "дюймов" },
        new TechSpec { Name = "Разрешение", Value = "3840x2160" },
        new TechSpec{ Name = "Тип матрицы", Value = "QLED" },
        new TechSpec { Name = "HDR", Value = "HDR10+" }
    } },
                new Product { Name = "Телевизор LG OLED", Price = 70000, ImageUrl = "tv2.png", Type = "TV", Specs = new List<TechSpec>{
        new TechSpec { Name = "Диагональ", Value = "70", Unit = "дюймов" },
        new TechSpec { Name = "Разрешение", Value = "3840x2160" },
        new TechSpec{ Name = "Тип матрицы", Value = "OLED" },
        new TechSpec { Name = "HDR", Value = "HDR10+" }
    }  },
               // new Product { Name = "Ноутбук ASUS ROG", Price = 90000, ImageUrl = "laptop1.png", CategoryId = "Laptops" },
              //  new Product { Name = "Ноутбук MacBook Pro", Price = 120000, ImageUrl = "laptop2.png", CategoryId = "Laptops" },
                new Product { Name = "Смартфон iPhone 14", Price = 80000, ImageUrl = "phone1.png", Type = "Smartphones",  Specs = new List<TechSpec>
    {
        new TechSpec { Name = "Диагональ экрана", Value = "6.1", Unit = "дюймов" },
        new TechSpec { Name = "Процессор", Value = "A16" },
        new TechSpec { Name = "Объем памяти", Value = "256", Unit = "ГБ" },
        new TechSpec{ Name = "Основная камера", Value = "48 Мп" },
        new TechSpec { Name = "Аккумулятор", Value = "3274", Unit = "мАч" }
    }},
                new Product { Name = "Смартфон Samsung Galaxy S22", Price = 60000, ImageUrl = "phone2.png", Type = "Smartphones" ,  Specs = new List<TechSpec>
    {
        new TechSpec { Name = "Диагональ экрана", Value = "6.1", Unit = "дюймов" },
        new TechSpec { Name = "Процессор", Value = "Qualcomm Snapdragon 8 Gen 1+" },
        new TechSpec { Name = "Объем памяти", Value = "500", Unit = "ГБ" },
        new TechSpec{ Name = "Основная камера", Value = "48 Мп" },
        new TechSpec { Name = "Аккумулятор", Value = "3500", Unit = "мАч" }
    }},
                new Product { Name = "Холодильник Bosch", Price = 60000, ImageUrl = "fridge1.png", Type = "Appliances",  Specs = new List<TechSpec>
    {
        new TechSpec { Name = "Общий объем", Value = "388", Unit = "л" },
        new TechSpec { Name = "Класс энергопотребления", Value = "A+" },
        new TechSpec { Name = "Количество камер", Value = "2" },
        new TechSpec { Name = "No Frost", Value = "Да" },
        new TechSpec { Name = "Уровень шума", Value = "38", Unit = "дБ" }
    } },
            };

            // Изначально отображаем все товары
            FilteredProducts = new ObservableCollection<Product>(AllProducts);
            
        }
       
        private void AddToCart(Product product)
        {
           
            CartViewModel.Instance.AddToCart(new CartItem { Product = SelectedProduct, Quantity = 1 });
        }
        private void OnResetFilter()
        {
            // Сбрасываем выбранную категорию
            SelectedCategory = null;

            // Отображаем все товары
            FilteredProducts = new ObservableCollection<Product>(AllProducts);
          
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

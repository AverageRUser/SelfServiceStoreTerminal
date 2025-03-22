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
   
    class CategoryViewModel : INotifyPropertyChanged
    {
        public ICommand SelectCategoryCommand => new Command(OnSelectCategory);
        private Category _selectedCategory;
        public ObservableCollection<Category> Categories { get; set; } = new()
        {
             new Category { Name = "Телевизоры", ImageUrl = "tv_category.png", CategoryId = "TV" },
                new Category { Name = "Ноутбуки", ImageUrl = "laptop_category.png", CategoryId = "Laptops" },
                new Category { Name = "Смартфоны", ImageUrl = "phone_category.png", CategoryId = "Smartphones" },
                new Category { Name = "Бытовая техника", ImageUrl = "appliance_category.png", CategoryId = "Appliances" }

         };
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }
        public CategoryViewModel()
        {
           
           
        }
        private void OnSelectCategory()
        {
            if (SelectedCategory != null)
            {
                MessagingCenter.Send(this, "CategorySelected", SelectedCategory.CategoryId);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

}

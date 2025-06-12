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
using TradeCompApp.Database;
using TradeCompApp.Models;

namespace TradeCompApp.ViewModels
{
   
    class CategoryViewModel : INotifyPropertyChanged
    {
        public ICommand SelectCategoryCommand => new Command(OnSelectCategory);
        private Category _selectedCategory;
        private ObservableCollection<Category> _category;
        private readonly DatabaseService _databaseService;
        public ObservableCollection<Category> Categories
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        

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
            _databaseService = new DatabaseService();
            LoadCategories();
        }
        public async void LoadCategories()
        {
            if (DatabaseStatus.IsConnected)
            {
                var category = await _databaseService.GetAllCategories();
                Categories = new ObservableCollection<Category>(category);
            }
            else
            {
                Categories = new ObservableCollection<Category>() {
                new Category { Name = "Телевизоры", ImageUrl = "tv1.png", Id = 1 },
            new Category { Name = "Смартфоны", ImageUrl = "phone1.png", Id = 2 },
            new Category { Name = "Бытовая техника", ImageUrl = "appliance_category.png", Id = 3 }
            };
            }
        }
        private void OnSelectCategory()
        {
            if (SelectedCategory != null)
            {
                MessagingCenter.Send(this, "CategorySelected", SelectedCategory.Id);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

}

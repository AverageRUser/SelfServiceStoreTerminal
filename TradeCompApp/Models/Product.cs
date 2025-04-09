using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TradeCompApp.Models
{
    public class Product : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _name;
        private decimal _price;
        private string _imagepath;
        private int _categoryId;

        public int ProductId { get; set; }

        public string Name { 
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
      
        public string ImageUrl
        {
            get => _imagepath;
            set
            {
                if (_imagepath != value)
                {
                    _imagepath = value;
                    OnPropertyChanged();
                }
            }
        }
      
        public int CategoryId
        {
            get => _categoryId;
            set
            {
                if (_categoryId != value)
                {
                    _categoryId = value;
                    OnPropertyChanged();
                }
            }
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public List<TechSpec> Specs { get; set; }
    }
    public class TechSpec
    {
     
        public int TechSpecId { get; set; }
    
        public string Name { get; set; }


        public string Value { get; set; }

        public string Unit { get; set; }

        public int ProductId { get; set; }

    }



}

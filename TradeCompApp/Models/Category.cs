using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCompApp.Models
{
    public class Category
    {
     
        public string Name { get; set; }

        public string ImageUrl { get; set; }
  
        public int Id {  get; set; }
    }
}


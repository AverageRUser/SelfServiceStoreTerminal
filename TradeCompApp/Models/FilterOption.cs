using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCompApp.Models
{
    public class FilterOption
    {
        public string Name { get; set; }
        public List<string> AvailableValues { get; set; }
        public string SelectedValue { get; set; }
    }
}

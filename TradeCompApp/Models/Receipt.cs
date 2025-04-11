using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCompApp.Models
{
    public class Receipt
    {
        public int Id {  get; set; }
        public DateTime CreatedAt { get; set; }
        public string INN { get; set; }
        public string KKM { get; set; }
        public string EKLZ { get; set; }
        public string FIO { get; set; }

        public string Position { get; set; }
        public string Address { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueDiamond
{
    public class Bill
    {
        public int id { get; set; }
        public string productName { get; set; }
        public Double Quantity { get; set; }
        public Double TP { get; set; }
        public Double Rate { get; set; }
        public Double Discount { get; set; }
        public Double Amount { get; set; }
        public Double NP { get; set; }
        public string Scheme { get; set; }

    }
}

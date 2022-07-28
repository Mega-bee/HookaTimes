using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels.Restaurant
{
    public partial class StocksList_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Quantity { get; set; }
        public string Status { get; set; }
    }

    public partial class Stock_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Quantity { get; set; }
        public string Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels.DeliveryCenter
{
    public partial class ProductsList_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string UnitPrice { get; set; }
    }

    public partial class Product_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string UnitPrice { get; set; }
        public string Description { get; set; }
    }

}

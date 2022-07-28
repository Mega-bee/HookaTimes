using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels.Restaurant
{
    public partial class OrdersList_VM
    {
        public int Id { get; set; }
        public string CreatedDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }

    public partial class Order_VM
    {
        public int Id { get; set; }
        public string CreatedDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}

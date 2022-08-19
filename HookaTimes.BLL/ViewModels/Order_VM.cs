using HookaTimes.BLL.ViewModels.Website;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
   public partial class OrderHistory_VM
    {
        public List<OrderHistoryMVC_VM> CurrentOrders { get; set; }
        public List<OrderHistoryMVC_VM> AllOrders { get; set; }
    }

    public partial class OrderDetails_VM : OrderHistoryMVC_VM
    {
        public List<CartItem_VM> Items { get; set; }
        public BuddyProfileAddressVM Address { get; set; }
    }



}

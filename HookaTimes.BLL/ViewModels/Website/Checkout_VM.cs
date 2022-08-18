using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels.Website
{
    public partial class Checkout_VM
    {
        public CartSummary_VM CartSummary { get; set; }
        public BuddyProfileAddressVM Address { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels.Concession
{
    public partial class SalesList_VM
    {
        public int OrderId { get; set; }

        public string OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerOrder { get; set; }
        public string Total { get; set; }

    }

}

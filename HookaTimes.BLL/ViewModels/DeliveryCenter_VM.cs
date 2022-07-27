using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class DeliveryCenter_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonInCharge { get; set; }
        public string EmailAddress { get; set; }
        public string Balance { get; set; }
    }

    public partial class DeliveryCenterList_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Balance { get; set; }
    }
}

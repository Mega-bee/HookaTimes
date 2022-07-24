using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class BaseDistributor_VM
    {
        [JsonProperty(Order = -2)]
        public int Id { get; set; }
        [JsonProperty(Order = -2)]
        public string Name { get; set; }
    }
    public partial class Distributor_VM : BaseDistributor_VM
    {
        public string CompanyName { get; set; }
        public string PersonInCharge { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }


    public partial class DistributorList_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateJoined { get; set; }
        public string Balance { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class BaseRestaurant_VM
    {
        [JsonProperty(Order = -2)]
        public int Id { get; set; }
        [JsonProperty(Order = -2)]
        public string Name { get; set; }
    }
    public partial class Restaurant_VM : BaseRestaurant_VM
    {
        public string PersonInCharge { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }


    public partial class RestaurantList_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DateJoined { get; set; }
        public string Balance { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels.Website
{
    public partial class PlacesPage_VM
    {
        public List<HookaPlaces_VM> Places { get; set; }
        public List<Cuisine_VM> Cuisines { get; set; }
    }
}

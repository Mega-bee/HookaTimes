using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels.Frontend
{
    public partial class HomePage_VM 
    {
        public List<HookaPlaces_VM> Places { get; set; }
        public List<HookaBuddy_VM> Buddies { get; set; }    
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class Invitation_VM
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public int OptionId { get; set; }
        public string Description { get; set; }
        public int ToBuddyId { get; set; }
        public int PlaceId { get; set; }
    }
}

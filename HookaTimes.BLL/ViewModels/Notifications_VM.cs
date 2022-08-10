using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class SentNotifications_VM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int NumbBuddies { get; set; }
    }

    public partial class InvDetail_VM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public double? Rating { get; set; }
        public string LocationTitle { get; set; }
        public List<buddies_VM> Buddies { get; set; }
    }

    public partial class buddies_VM
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Status { get; set; }
    }

}

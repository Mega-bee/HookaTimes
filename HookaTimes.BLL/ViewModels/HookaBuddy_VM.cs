using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class HookaBuddy_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public bool? IsAvailable { get; set; }
        public float Rating { get; set; }
        public string Image { get; set; }
        public bool HasPendingInvite { get; set; }
    }
}

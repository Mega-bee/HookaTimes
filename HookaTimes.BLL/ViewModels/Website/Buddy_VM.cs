using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels.Website
{
    public partial class Buddy_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Rating { get; set; }
        public string Image { get; set; }
        public string Profession { get; set; }
        public string Address { get; set; }
    }
}

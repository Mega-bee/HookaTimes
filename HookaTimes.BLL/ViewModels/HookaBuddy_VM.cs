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
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Image { get; set; }
    }

    public partial class HookaBuddiesList_VM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
    }
}

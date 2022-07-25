using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class Concession_VM
    {
        public int Id { get; set; }
        public string Dimension { get; set; }
        public string Capacity { get; set; }
        public string EmailAddress { get; set; }
        public string Status { get; set; }
    }


    public partial class ConcessionList_VM
    {
        public int Id { get; set; }
        public string DateJoined { get; set; }
        public string Status { get; set; }
    }
}

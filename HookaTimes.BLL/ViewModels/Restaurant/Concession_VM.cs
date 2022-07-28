using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels.Restaurant
{
    public partial class ConcessionsList_VM
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public string Total { get; set; }
    }

    public partial class Concession_VM
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public string Total { get; set; }
    }
}

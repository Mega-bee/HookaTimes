using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels.Restaurant
{
    public partial class ReservationsList_VM
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string CustomerName { get; set; }
        public int NumberOfPpl { get; set; }
        public string Remarks { get; set; }
    }

    public partial class Reservation_VM
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string CustomerName { get; set; }
        public int NumberOfPpl { get; set; }
        public string Remarks { get; set; }
    }
}

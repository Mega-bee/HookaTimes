using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class Wishlist_VM
    {
        public int? ItemId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal? ProductPrice { get; set; }
    }
}

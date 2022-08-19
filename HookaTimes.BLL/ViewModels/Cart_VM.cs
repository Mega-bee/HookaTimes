using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class CartSummary_VM
    {
        public decimal? TotalPrice { get; set; }
        public List<CartItem_VM> Items { get; set; }
    }
    public partial class CartItem_VM
    {
        public int? ItemId { get; set; }
        public int CategoryId { get; set; }
        public int? Quantity { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal? ProductPrice { get; set; }
        public decimal? TotalPrice { get; set; }
    }

    public partial class UpdateCartItem_VM
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}

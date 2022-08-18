using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels.Website
{
    public partial class ViewHookaProduct_VM
    {
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryImage { get; set; }
        public List<HookaProduct_VM> Products { get; set; }
        public List<Product_VM> RelatedCategories { get; set; }

    }

    public partial class HookaProduct_VM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public decimal? CustomerFinalPrice { get; set; }
        public string Description { get; set; }
        public bool IsInWishlist { get; set; }
    }

    public partial class RelatedCategory_VM
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
    }
}

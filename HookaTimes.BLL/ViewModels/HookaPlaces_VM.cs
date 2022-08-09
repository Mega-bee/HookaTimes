using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class HookaPlaces_VM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public string Cuisine { get; set; }
        public float Rating { get; set; }
        public string Name { get; set; }
    }

    public partial class HookaPlaceInfo_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public float Rating { get; set; }
        public string Location { get; set; }
        public string Cuisine { get; set; }
        public string OpeningFrom { get; set; }
        public string OpeningTo { get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
        public List<HookaPlaceFavorite_VM> Favorites { get; set; }
        public List<HookaPlaceImage_VM> Albums { get; set; }
        public List<HookaPlaceImage_VM> Menus { get; set; }
        public List<HookaPlaceReview_VM> Reviews { get; set; }
    }

    public partial class HookaPlaceFavorite_VM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public bool? IsAvailable { get; set; }
    }

    public partial class HookaPlaceReview_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public float Rating { get; set; }
        public string Description { get; set; }
    }

    public partial class HookaPlaceImage_VM
    {
        public int Id { get; set; }
        public string Image { get; set; }
    }

 
}

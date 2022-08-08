using System;

namespace HookaTimes.BLL.ViewModels
{
    public partial class OfferList_VM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public float Rating { get; set; }
        public string RestaurantTitle { get; set; }
    }


    public partial class Offer_VM
    {
        public string Image { get; set; }
        public string OfferTitle { get; set; }
        public string RestaurantName { get; set; }
        public string Location { get; set; }
        public string Rating { get; set; }
        public string OfferDescription { get; set; }
        public string RestaurantDescription { get; set; }
        public string Cuisine { get; set; }
        public string OpenningFrom { get; set; }
        public string OpenningTo { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string PhonNumber { get; set; }
        public string Type { get; set; }
        public string Discount { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
}

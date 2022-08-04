using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("PlacesProfile")]
    public partial class PlacesProfile
    {
        public PlacesProfile()
        {
            FavoriteUserPlaces = new HashSet<FavoriteUserPlace>();
            PlaceAlbums = new HashSet<PlaceAlbum>();
            PlaceMenus = new HashSet<PlaceMenu>();
            PlaceOffers = new HashSet<PlaceOffer>();
            PlaceReviews = new HashSet<PlaceReview>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Image { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? LocationId { get; set; }
        public int? CuisineId { get; set; }
        public double? Rating { get; set; }
        [StringLength(63)]
        public string OpenningFrom { get; set; }
        [StringLength(63)]
        public string OpenningTo { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [StringLength(255)]
        public string Longitude { get; set; }
        [StringLength(255)]
        public string Latitude { get; set; }
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [ForeignKey("CuisineId")]
        [InverseProperty("PlacesProfiles")]
        public virtual Cuisine Cuisine { get; set; }
        [ForeignKey("LocationId")]
        [InverseProperty("PlacesProfiles")]
        public virtual Location Location { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("PlacesProfiles")]
        public virtual AspNetUser User { get; set; }
        [InverseProperty("PlaceProfile")]
        public virtual ICollection<FavoriteUserPlace> FavoriteUserPlaces { get; set; }
        [InverseProperty("PlaceProfile")]
        public virtual ICollection<PlaceAlbum> PlaceAlbums { get; set; }
        [InverseProperty("PlaceProfile")]
        public virtual ICollection<PlaceMenu> PlaceMenus { get; set; }
        [InverseProperty("PlaceProfile")]
        public virtual ICollection<PlaceOffer> PlaceOffers { get; set; }
        [InverseProperty("PlaceProfile")]
        public virtual ICollection<PlaceReview> PlaceReviews { get; set; }
    }
}

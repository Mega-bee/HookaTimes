using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    public partial class FavoriteUserPlace
    {
        [Key]
        public int Id { get; set; }
        public int? BuddyId { get; set; }
        public int? PlaceProfileId { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("BuddyId")]
        [InverseProperty("FavoriteUserPlaces")]
        public virtual BuddyProfile? Buddy { get; set; }
        [ForeignKey("PlaceProfileId")]
        [InverseProperty("FavoriteUserPlaces")]
        public virtual PlacesProfile? PlaceProfile { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("PlaceReview")]
    public partial class PlaceReview
    {
        [Key]
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? BuddyId { get; set; }
        public int? PlaceProfileId { get; set; }
        public double? Rating { get; set; }
        public string? Description { get; set; }

        [ForeignKey("BuddyId")]
        [InverseProperty("PlaceReviews")]
        public virtual BuddyProfile? Buddy { get; set; }
        [ForeignKey("PlaceProfileId")]
        [InverseProperty("PlaceReviews")]
        public virtual PlacesProfile? PlaceProfile { get; set; }
    }
}

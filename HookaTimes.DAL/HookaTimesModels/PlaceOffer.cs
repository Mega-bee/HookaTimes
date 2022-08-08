using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("PlaceOffer")]
    public partial class PlaceOffer
    {
        [Key]
        public int Id { get; set; }
        public int? PlaceProfileId { get; set; }
        [StringLength(255)]
        public string Image { get; set; }
        [StringLength(2047)]
        public string Description { get; set; }
        public int? TypeId { get; set; }
        public int? Discount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ExpiryDate { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(511)]
        public string Title { get; set; }

        [ForeignKey("PlaceProfileId")]
        [InverseProperty("PlaceOffers")]
        public virtual PlacesProfile PlaceProfile { get; set; }
        [ForeignKey("TypeId")]
        [InverseProperty("PlaceOffers")]
        public virtual OfferType Type { get; set; }
    }
}

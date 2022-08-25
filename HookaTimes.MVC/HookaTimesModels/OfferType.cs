using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("OfferType")]
    public partial class OfferType
    {
        public OfferType()
        {
            PlaceOffers = new HashSet<PlaceOffer>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string? Title { get; set; }

        [InverseProperty("Type")]
        public virtual ICollection<PlaceOffer> PlaceOffers { get; set; }
    }
}

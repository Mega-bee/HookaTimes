using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("Wishlist")]
    public partial class Wishlist
    {
        [Key]
        public int Id { get; set; }
        public int? BuddyId { get; set; }
        public int? ProductId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey("BuddyId")]
        [InverseProperty("Wishlists")]
        public virtual BuddyProfile? Buddy { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("Wishlists")]
        public virtual Product? Product { get; set; }
    }
}

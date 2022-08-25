using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("VirtualWishlist")]
    public partial class VirtualWishlist
    {
        [Key]
        public int Id { get; set; }
        [StringLength(450)]
        public string? WishlistSessionId { get; set; }
        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("VirtualWishlists")]
        public virtual Product? Product { get; set; }
    }
}

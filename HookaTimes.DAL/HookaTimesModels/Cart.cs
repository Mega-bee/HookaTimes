using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("Cart")]
    public partial class Cart
    {
        [Key]
        public int Id { get; set; }
        public int? BuddyId { get; set; }
        public int? ProductId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public int? Quantity { get; set; }

        [ForeignKey("BuddyId")]
        [InverseProperty("Carts")]
        public virtual BuddyProfile Buddy { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("Carts")]
        public virtual Product Product { get; set; }
    }
}

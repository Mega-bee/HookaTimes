using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("VirtualCart")]
    public partial class VirtualCart
    {
        [Key]
        public int Id { get; set; }
        [StringLength(450)]
        public string? SessionCartId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("VirtualCarts")]
        public virtual Product? Product { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("Order")]
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        [Key]
        public int Id { get; set; }
        public int? OrderStatusId { get; set; }
        public int? BuddyId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Total { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Subtotal { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Fees { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public int? AddressId { get; set; }

        [ForeignKey("AddressId")]
        [InverseProperty("Orders")]
        public virtual BuddyProfileAddress Address { get; set; }
        [ForeignKey("BuddyId")]
        [InverseProperty("Orders")]
        public virtual BuddyProfile Buddy { get; set; }
        [ForeignKey("OrderStatusId")]
        [InverseProperty("Orders")]
        public virtual OrderStatus OrderStatus { get; set; }
        [InverseProperty("Order")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}

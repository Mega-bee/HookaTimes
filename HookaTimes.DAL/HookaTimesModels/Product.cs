using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            OrderItems = new HashSet<OrderItem>();
            VirtualCarts = new HashSet<VirtualCart>();
        }

        [Key]
        public int Id { get; set; }
        public int? ProductCategoryId { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ManufacturerPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RegionalPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DistributorPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PosPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CustomerInitialPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CustomerDiscount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CustomerFinalPrice { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsRawMaterial { get; set; }
        public bool? IsForSale { get; set; }
        public string Image { get; set; }

        [ForeignKey("ProductCategoryId")]
        [InverseProperty("Products")]
        public virtual ProductCategory ProductCategory { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Cart> Carts { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<VirtualCart> VirtualCarts { get; set; }
    }
}

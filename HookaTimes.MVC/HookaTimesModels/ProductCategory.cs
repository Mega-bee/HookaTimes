using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("ProductCategory")]
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string? Title { get; set; }
        [StringLength(255)]
        public string? Image { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public string? Description { get; set; }

        [InverseProperty("ProductCategory")]
        public virtual ICollection<Product> Products { get; set; }
    }
}

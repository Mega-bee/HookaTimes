using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("BuddyProfileAddress")]
    public partial class BuddyProfileAddress
    {
        public BuddyProfileAddress()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        public int Id { get; set; }
        public int? BuddyProfileId { get; set; }
        [StringLength(255)]
        public string Longitude { get; set; }
        [StringLength(255)]
        public string Latitude { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(255)]
        public string Apartment { get; set; }
        [StringLength(255)]
        public string Building { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(255)]
        public string Street { get; set; }

        [ForeignKey("BuddyProfileId")]
        [InverseProperty("BuddyProfileAddresses")]
        public virtual BuddyProfile BuddyProfile { get; set; }
        [InverseProperty("Address")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}

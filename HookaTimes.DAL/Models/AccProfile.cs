using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    [Table("ACC_Profiles")]
    public partial class AccProfile
    {
        [Key]
        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [StringLength(250)]
        public string Email { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        public int RoleId { get; set; }
        [Column("isDeleted")]
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }
        public int? GenderId { get; set; }
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(GenderId))]
        [InverseProperty("AccProfiles")]
        public virtual Gender Gender { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(AccProfileRole.AccProfiles))]
        public virtual AccProfileRole Role { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUser.AccProfiles))]
        public virtual AspNetUser User { get; set; }
    }
}

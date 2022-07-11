using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    [Table("ACC_ProfileRoles")]
    public partial class AccProfileRole
    {
        public AccProfileRole()
        {
            AccProfiles = new HashSet<AccProfile>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(250)]
        public string RoleName { get; set; }

        [InverseProperty(nameof(AccProfile.Role))]
        public virtual ICollection<AccProfile> AccProfiles { get; set; }
    }
}

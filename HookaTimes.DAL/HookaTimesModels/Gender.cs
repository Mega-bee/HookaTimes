using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("Gender")]
    public partial class Gender
    {
        public Gender()
        {
            BuddyProfiles = new HashSet<BuddyProfile>();
        }

        [Key]
        public int Id { get; set; }
        [Column("TItle")]
        [StringLength(31)]
        public string Title { get; set; }
        public bool? IsDeleted { get; set; }

        [InverseProperty("GenderNavigation")]
        public virtual ICollection<BuddyProfile> BuddyProfiles { get; set; }
    }
}

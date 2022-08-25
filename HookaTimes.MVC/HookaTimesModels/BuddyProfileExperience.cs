using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("BuddyProfileExperience")]
    public partial class BuddyProfileExperience
    {
        [Key]
        public int Id { get; set; }
        public int? BuddyProfileId { get; set; }
        [StringLength(255)]
        public string? Place { get; set; }
        [StringLength(255)]
        public string? Position { get; set; }
        [StringLength(255)]
        public string? WorkedFrom { get; set; }
        [Column("workedTo")]
        [StringLength(255)]
        public string? WorkedTo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("BuddyProfileId")]
        [InverseProperty("BuddyProfileExperiences")]
        public virtual BuddyProfile? BuddyProfile { get; set; }
    }
}

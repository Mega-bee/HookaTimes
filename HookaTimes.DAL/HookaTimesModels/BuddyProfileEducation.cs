using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("BuddyProfileEducation")]
    public partial class BuddyProfileEducation
    {
        [Key]
        public int Id { get; set; }
        public int? BuddyProfileId { get; set; }
        [StringLength(511)]
        public string University { get; set; }
        [StringLength(511)]
        public string Degree { get; set; }
        [StringLength(255)]
        public string StudiedFrom { get; set; }
        [StringLength(255)]
        public string StudiedTo { get; set; }

        [ForeignKey("BuddyProfileId")]
        [InverseProperty("BuddyProfileEducations")]
        public virtual BuddyProfile BuddyProfile { get; set; }
    }
}

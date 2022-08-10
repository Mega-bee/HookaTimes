﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("BuddyProfileAddress")]
    public partial class BuddyProfileAddress
    {
        [Key]
        public int Id { get; set; }
        public int? BuddyProfileId { get; set; }
        [StringLength(255)]
        public string Longitude { get; set; }
        [StringLength(255)]
        public string Latitude { get; set; }
        [StringLength(255)]
        public string Title { get; set; }

        [ForeignKey("BuddyProfileId")]
        [InverseProperty("BuddyProfileAddresses")]
        public virtual BuddyProfile BuddyProfile { get; set; }
    }
}
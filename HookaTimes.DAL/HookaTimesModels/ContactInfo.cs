using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("ContactInfo")]
    public partial class ContactInfo
    {
        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string SocialMediaLink1 { get; set; }
        [StringLength(255)]
        public string SocialMediaLink2 { get; set; }
        [StringLength(255)]
        public string SocialMediaLink3 { get; set; }
    }
}

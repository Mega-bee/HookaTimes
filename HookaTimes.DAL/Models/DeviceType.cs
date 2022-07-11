using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    [Table("DeviceType")]
    public partial class DeviceType
    {
        [Key]
        public int Id { get; set; }
        [StringLength(10)]
        public string Title { get; set; }
        public bool? IsWearable { get; set; }
    }
}

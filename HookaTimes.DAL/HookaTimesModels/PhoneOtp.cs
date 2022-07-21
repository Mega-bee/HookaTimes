using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("PhoneOtp")]
    public partial class PhoneOtp
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [StringLength(6)]
        public string Otp { get; set; }
    }
}

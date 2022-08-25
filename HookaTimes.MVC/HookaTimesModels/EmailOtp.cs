using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("EmailOtp")]
    public partial class EmailOtp
    {
        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string? Email { get; set; }
        [StringLength(6)]
        public string? Otp { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("Gender")]
    public partial class Gender
    {
        [Key]
        public int Id { get; set; }
        [Column("TItle")]
        [StringLength(31)]
        public string? Title { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

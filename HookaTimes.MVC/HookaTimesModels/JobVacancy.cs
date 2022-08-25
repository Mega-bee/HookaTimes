using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("JobVacancy")]
    public partial class JobVacancy
    {
        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string? Title { get; set; }
        [StringLength(450)]
        public string? Description { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}

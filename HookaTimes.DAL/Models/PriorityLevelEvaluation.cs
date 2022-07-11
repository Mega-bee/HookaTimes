using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    [Table("PriorityLevelEvaluation")]
    public partial class PriorityLevelEvaluation
    {
        [Key]
        public int Id { get; set; }
        public int? Risk1 { get; set; }
        public int? Risk2 { get; set; }
        public int? Risk3 { get; set; }
        public int? PriorityLevel { get; set; }
    }
}

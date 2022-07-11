using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    [Table("MetricRange")]
    public partial class MetricRange
    {
        [Key]
        public int Id { get; set; }
        public int? MetricTypeId { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public int? RiskLevel { get; set; }
        [StringLength(255)]
        public string Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    public partial class MetricsNormalValue
    {
        public MetricsNormalValue()
        {
            MetricLogs = new HashSet<MetricLog>();
        }

        [Key]
        public int Id { get; set; }
        public int? MetricTypeId { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public int? RiskLevel { get; set; }
        [StringLength(255)]
        public string Status { get; set; }
        public int? PatientId { get; set; }

        [ForeignKey(nameof(MetricTypeId))]
        [InverseProperty("MetricsNormalValues")]
        public virtual MetricType MetricType { get; set; }
        [ForeignKey(nameof(PatientId))]
        [InverseProperty("MetricsNormalValues")]
        public virtual Patient Patient { get; set; }
        [InverseProperty(nameof(MetricLog.Status))]
        public virtual ICollection<MetricLog> MetricLogs { get; set; }
    }
}

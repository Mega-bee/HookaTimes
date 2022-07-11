using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    [Table("MetricLog")]
    public partial class MetricLog
    {
        [Key]
        public int Id { get; set; }
        public int? MetricTypeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        public int? PatientId { get; set; }
        public double? MetricValue { get; set; }
        public int? StatusId { get; set; }

        [ForeignKey(nameof(MetricTypeId))]
        [InverseProperty("MetricLogs")]
        public virtual MetricType MetricType { get; set; }
        [ForeignKey(nameof(PatientId))]
        [InverseProperty("MetricLogs")]
        public virtual Patient Patient { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty(nameof(MetricsNormalValue.MetricLogs))]
        public virtual MetricsNormalValue Status { get; set; }
    }
}

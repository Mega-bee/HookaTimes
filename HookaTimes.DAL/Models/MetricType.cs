using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    [Table("MetricType")]
    public partial class MetricType
    {
        public MetricType()
        {
            Anomalies = new HashSet<Anomaly>();
            MetricLogs = new HashSet<MetricLog>();
            MetricsNormalValues = new HashSet<MetricsNormalValue>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        public bool? IsDeleted { get; set; }
        [StringLength(255)]
        public string UnitOfMeasure { get; set; }

        [InverseProperty(nameof(Anomaly.MetricType))]
        public virtual ICollection<Anomaly> Anomalies { get; set; }
        [InverseProperty(nameof(MetricLog.MetricType))]
        public virtual ICollection<MetricLog> MetricLogs { get; set; }
        [InverseProperty(nameof(MetricsNormalValue.MetricType))]
        public virtual ICollection<MetricsNormalValue> MetricsNormalValues { get; set; }
    }
}

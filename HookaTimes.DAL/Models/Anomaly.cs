using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    [Table("Anomaly")]
    public partial class Anomaly
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        [StringLength(450)]
        public string MonitoredBy { get; set; }
        public double? Value { get; set; }
        public int? MetricTypeId { get; set; }
        public int? PatientId { get; set; }

        [ForeignKey(nameof(MetricTypeId))]
        [InverseProperty("Anomalies")]
        public virtual MetricType MetricType { get; set; }
        [ForeignKey(nameof(MonitoredBy))]
        [InverseProperty(nameof(AspNetUser.Anomalies))]
        public virtual AspNetUser MonitoredByNavigation { get; set; }
        [ForeignKey(nameof(PatientId))]
        [InverseProperty("Anomalies")]
        public virtual Patient Patient { get; set; }
    }
}

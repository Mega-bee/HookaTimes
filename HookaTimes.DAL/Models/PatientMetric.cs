using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    public partial class PatientMetric
    {
        public PatientMetric()
        {
            Anomalies = new HashSet<Anomaly>();
        }

        [Key]
        public int Id { get; set; }
        [Column("IMEI")]
        public string Imei { get; set; }
        public double? BloodOxygen { get; set; }
        public double? BloodPressure { get; set; }
        public double? BodyTemperature { get; set; }
        public double? HeartRate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        public bool? IsDeleted { get; set; }

        //[InverseProperty(nameof(Anomaly.Log))]
        public virtual ICollection<Anomaly> Anomalies { get; set; }
    }
}

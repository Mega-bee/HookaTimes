using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    [Table("Patient")]
    public partial class Patient
    {
        public Patient()
        {
            Anomalies = new HashSet<Anomaly>();
            MetricLogs = new HashSet<MetricLog>();
            MetricsNormalValues = new HashSet<MetricsNormalValue>();
            PatientDevices = new HashSet<PatientDevice>();
            PatientMedicines = new HashSet<PatientMedicine>();
        }

        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public int? GenderId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateOfBirth { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AdmissionDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ScheduledReleaseDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int? Priority { get; set; }

        [ForeignKey(nameof(GenderId))]
        [InverseProperty("Patients")]
        public virtual Gender Gender { get; set; }
        [InverseProperty(nameof(Anomaly.Patient))]
        public virtual ICollection<Anomaly> Anomalies { get; set; }
        [InverseProperty(nameof(MetricLog.Patient))]
        public virtual ICollection<MetricLog> MetricLogs { get; set; }
        [InverseProperty(nameof(MetricsNormalValue.Patient))]
        public virtual ICollection<MetricsNormalValue> MetricsNormalValues { get; set; }
        [InverseProperty(nameof(PatientDevice.Patient))]
        public virtual ICollection<PatientDevice> PatientDevices { get; set; }
        [InverseProperty(nameof(PatientMedicine.Patient))]
        public virtual ICollection<PatientMedicine> PatientMedicines { get; set; }
    }
}

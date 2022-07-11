using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    public partial class RawPatientMetric
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column("IMEI")]
        public string Imei { get; set; }
        public string BloodOxygen { get; set; }
        public string BloodPressure { get; set; }
        public string BodyTemperature { get; set; }
        public string HeartRate { get; set; }
        public string DateCreated { get; set; }
        public bool? IsDeleted { get; set; }
        public string BloodPressureSystolic { get; set; }
        public string BloodPressureDiastolic { get; set; }
    }
}
